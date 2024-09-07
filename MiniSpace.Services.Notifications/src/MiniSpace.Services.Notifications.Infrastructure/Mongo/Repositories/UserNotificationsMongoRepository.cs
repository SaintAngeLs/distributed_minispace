using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public class UserNotificationsMongoRepository : IExtendedUserNotificationsRepository
    {
        private readonly IMongoRepository<UserNotificationsDocument, Guid> _repository;

        public UserNotificationsMongoRepository(IMongoRepository<UserNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserNotifications> GetByUserIdAsync(Guid studentId)
        {
            var document = await _repository.GetAsync(d => d.UserId == studentId);
            return document?.AsEntity();
        }

        public async Task AddAsync(UserNotifications studentNotifications)
        {
            var document = studentNotifications.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(UserNotifications studentNotifications)
        {
            var filter = Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentNotifications.UserId);
            var update = Builders<UserNotificationsDocument>.Update
                .SetOnInsert(doc => doc.Id, studentNotifications.UserId) 
                .PushEach(doc => doc.Notifications, studentNotifications.Notifications.Select(n => n.AsDocument()));

            var options = new UpdateOptions { IsUpsert = true };
            await _repository.Collection.UpdateOneAsync(filter, update, options);
        }

        public async Task AddOrUpdateAsync(UserNotifications studentNotifications)
        {
            var document = studentNotifications.AsDocument();
            var filter = Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentNotifications.UserId);

            var initializationUpdate = Builders<UserNotificationsDocument>.Update
                .SetOnInsert(doc => doc.Notifications, new List<NotificationDocument>())
                .SetOnInsert(doc => doc.Id, document.Id); 

            var addToSetUpdates = new List<UpdateDefinition<UserNotificationsDocument>>();
            foreach (var notification in studentNotifications.Notifications)
            {
                var notificationDocument = notification.AsDocument();
                var notificationFilter = Builders<UserNotificationsDocument>.Filter.And(
                    Builders<UserNotificationsDocument>.Filter.Eq("Notifications.Message", notificationDocument.Message),
                    Builders<UserNotificationsDocument>.Filter.Eq("Notifications.CreatedAt", notificationDocument.CreatedAt),
                    Builders<UserNotificationsDocument>.Filter.Eq("Notifications.EventType", notificationDocument.EventType)
                );

                var combinedFilter = Builders<UserNotificationsDocument>.Filter.And(filter, notificationFilter);
                var update = Builders<UserNotificationsDocument>.Update.AddToSet(doc => doc.Notifications, notificationDocument);

                addToSetUpdates.Add(update);
            }

            var options = new UpdateOptions { IsUpsert = true };

            await _repository.Collection.UpdateOneAsync(filter, initializationUpdate, options);

            foreach (var update in addToSetUpdates)
            {
                await _repository.Collection.UpdateOneAsync(filter, update, options);
            }
        }

       public async Task<List<UserNotifications>> FindAsync(FilterDefinition<UserNotificationsDocument> filter, FindOptions options)
        {
            var documents = await _repository.Collection.Find(filter, options).ToListAsync();
            return documents.Select(doc => doc.AsEntity()).ToList();
        }


        public Task DeleteAsync(Guid studentId)
        {
            return _repository.DeleteAsync(studentId);
        }

        public async Task<UpdateResult> BulkUpdateAsync(FilterDefinition<UserNotificationsDocument> filter, UpdateDefinition<UserNotificationsDocument> update)
        {
            return await _repository.Collection.UpdateManyAsync(filter, update);
        }

        public async Task UpdateNotificationStatus(Guid studentId, Guid notificationId, string newStatus)
        {
            var filter = Builders<UserNotificationsDocument>.Filter.And(
                Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentId),
                Builders<UserNotificationsDocument>.Filter.ElemMatch(doc => doc.Notifications, n => n.NotificationId == notificationId)
            );

            var update = Builders<UserNotificationsDocument>.Update
                .Set("Notifications.$.Status", newStatus)
                .CurrentDate("Notifications.$.UpdatedAt");

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
        
        public async Task<bool> NotificationExists(Guid studentId, Guid notificationId)
        {
            var filter = Builders<UserNotificationsDocument>.Filter.And(
                Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentId),
                Builders<UserNotificationsDocument>.Filter.ElemMatch(doc => doc.Notifications, n => n.NotificationId == notificationId)
            );

            var result = await _repository.Collection.Find(filter).AnyAsync();
            return result;
        }

        public async Task DeleteNotification(Guid studentId, Guid notificationId)
        {
            var filter = Builders<UserNotificationsDocument>.Filter.And(
                Builders<UserNotificationsDocument>.Filter.Eq(d => d.UserId, studentId),
                Builders<UserNotificationsDocument>.Filter.ElemMatch(e => e.Notifications, n => n.NotificationId == notificationId));

            var update = Builders<UserNotificationsDocument>.Update.PullFilter(
                p => p.Notifications, n => n.NotificationId == notificationId);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<int> GetNotificationCount(Guid studentId)
        {
            var studentNotifications = await _repository.GetAsync(x => x.UserId == studentId);
            return studentNotifications?.Notifications.Count ?? 0;
        }

         public async Task<List<NotificationDocument>> GetRecentNotifications(Guid studentId, int count)
        {
            var filter = Builders<UserNotificationsDocument>.Filter.Eq(d => d.UserId, studentId);

            var options = new FindOptions<UserNotificationsDocument>
            {
                Sort = Builders<UserNotificationsDocument>.Sort.Descending(d => d.Notifications[-1].CreatedAt),
                Projection = Builders<UserNotificationsDocument>.Projection.Slice(p => p.Notifications, -count)
            };

            var cursor = await _repository.Collection.FindAsync(filter, options);

            var documents = await cursor.ToListAsync();
            var notifications = new List<NotificationDocument>();
            foreach (var document in documents) 
            {
                if (document.Notifications != null)
                {
                    notifications.AddRange(document.Notifications);
                }
            }

            return notifications;
        }
    }
}
