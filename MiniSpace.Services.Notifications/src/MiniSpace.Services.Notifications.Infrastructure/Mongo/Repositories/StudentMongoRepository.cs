using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
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

        public Task UpdateAsync(Notification notification)
            => _repository.UpdateAsync(notification.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}
