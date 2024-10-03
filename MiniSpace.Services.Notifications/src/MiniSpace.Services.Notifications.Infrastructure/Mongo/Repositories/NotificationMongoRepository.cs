using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public class NotificationMongoRepository : INotificationRepository
    {
        private readonly IMongoRepository<NotificationDocument, Guid> _repository;

        public NotificationMongoRepository(IMongoRepository<NotificationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Notification> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(o => o.NotificationId == id);
            return document?.AsEntity();
        }

        public Task AddAsync(Notification notification)
            => _repository.AddAsync(notification.AsDocument());

         public async Task UpdateAsync(Notification notification)
        {
            var filter = Builders<NotificationDocument>.Filter.Eq(doc => doc.NotificationId, notification.NotificationId);
            var update = Builders<NotificationDocument>.Update
                .Set(doc => doc.Status, notification.Status.ToString())
                .Set(doc => doc.UpdatedAt, DateTime.UtcNow);

            var updateResult = await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}
