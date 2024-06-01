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
    public class StudentNotificationsMongoRepository : IExtendedStudentNotificationsRepository
    {
        private readonly IMongoRepository<StudentNotificationsDocument, Guid> _repository;

        public StudentNotificationsMongoRepository(IMongoRepository<StudentNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<StudentNotifications> GetByStudentIdAsync(Guid studentId)
        {
            var document = await _repository.GetAsync(d => d.StudentId == studentId);
            return document?.AsEntity();
        }

        public async Task AddAsync(StudentNotifications studentNotifications)
        {
            var document = studentNotifications.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(StudentNotifications studentNotifications)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentNotifications.StudentId);
            var update = Builders<StudentNotificationsDocument>.Update
                .SetOnInsert(doc => doc.Id, studentNotifications.StudentId) 
                .PushEach(doc => doc.Notifications, studentNotifications.Notifications.Select(n => n.AsDocument()));

            var options = new UpdateOptions { IsUpsert = true };
            await _repository.Collection.UpdateOneAsync(filter, update, options);
        }

         public async Task AddOrUpdateAsync(StudentNotifications studentNotifications)
        {
            var document = studentNotifications.AsDocument();
            var filter = Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentNotifications.StudentId);
            var updates = new List<UpdateDefinition<StudentNotificationsDocument>>();

            foreach (var notification in studentNotifications.Notifications)
            {
                updates.Add(Builders<StudentNotificationsDocument>.Update.Push(doc => doc.Notifications, notification.AsDocument()));
            }

            var update = Builders<StudentNotificationsDocument>.Update
                .SetOnInsert(doc => doc.Id, studentNotifications.StudentId) 
                .AddToSetEach(doc => doc.Notifications, studentNotifications.Notifications.Select(n => n.AsDocument())); 

            var options = new UpdateOptions { IsUpsert = true };
            await _repository.Collection.UpdateOneAsync(filter, update, options);
        }


       public async Task<List<StudentNotifications>> FindAsync(FilterDefinition<StudentNotificationsDocument> filter, FindOptions options)
        {
            var documents = await _repository.Collection.Find(filter, options).ToListAsync();
            return documents.Select(doc => doc.AsEntity()).ToList();
        }


        public Task DeleteAsync(Guid studentId)
        {
            return _repository.DeleteAsync(studentId);
        }

        public async Task<UpdateResult> BulkUpdateAsync(FilterDefinition<StudentNotificationsDocument> filter, UpdateDefinition<StudentNotificationsDocument> update)
        {
            return await _repository.Collection.UpdateManyAsync(filter, update);
        }

        public async Task UpdateNotificationStatus(Guid studentId, Guid notificationId, string newStatus)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.And(
                Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                Builders<StudentNotificationsDocument>.Filter.ElemMatch(doc => doc.Notifications, n => n.NotificationId == notificationId)
            );

            var update = Builders<StudentNotificationsDocument>.Update
                .Set("Notifications.$.Status", newStatus)
                .CurrentDate("Notifications.$.UpdatedAt");

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
        
        public async Task<bool> NotificationExists(Guid studentId, Guid notificationId)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.And(
                Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                Builders<StudentNotificationsDocument>.Filter.ElemMatch(doc => doc.Notifications, n => n.NotificationId == notificationId)
            );

            var result = await _repository.Collection.Find(filter).AnyAsync();
            return result;
        }

        public async Task DeleteNotification(Guid studentId, Guid notificationId)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.And(
                Builders<StudentNotificationsDocument>.Filter.Eq(d => d.StudentId, studentId),
                Builders<StudentNotificationsDocument>.Filter.ElemMatch(e => e.Notifications, n => n.NotificationId == notificationId));

            var update = Builders<StudentNotificationsDocument>.Update.PullFilter(
                p => p.Notifications, n => n.NotificationId == notificationId);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<int> GetNotificationCount(Guid studentId)
        {
            var studentNotifications = await _repository.GetAsync(x => x.StudentId == studentId);
            return studentNotifications?.Notifications.Count ?? 0;
        }

         public async Task<List<NotificationDocument>> GetRecentNotifications(Guid studentId, int count)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.Eq(d => d.StudentId, studentId);

            var options = new FindOptions<StudentNotificationsDocument>
            {
                Sort = Builders<StudentNotificationsDocument>.Sort.Descending(d => d.Notifications[-1].CreatedAt),
                Projection = Builders<StudentNotificationsDocument>.Projection.Slice(p => p.Notifications, -count)
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
