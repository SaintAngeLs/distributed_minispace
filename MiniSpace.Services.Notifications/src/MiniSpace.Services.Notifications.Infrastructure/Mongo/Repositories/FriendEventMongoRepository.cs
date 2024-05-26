using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Notifications.Core.Repositories;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public class FriendEventMongoRepository : IFriendEventRepository
    {
        private readonly IMongoRepository<FriendEventDocument, Guid> _repository;

        public FriendEventMongoRepository(IMongoRepository<FriendEventDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(FriendEvent friendEvent)
        {
            var document = friendEvent.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<FriendEvent> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(id);
            return document?.AsEntity();
        }

        public async Task UpdateAsync(FriendEvent friendEvent)
        {
            var document = friendEvent.AsDocument();
            var filter = Builders<FriendEventDocument>.Filter.Eq(doc => doc.Id, document.Id);
            var update = Builders<FriendEventDocument>.Update
                .Set(doc => doc.EventType, document.EventType)
                .Set(doc => doc.Details, document.Details)
                .Set(doc => doc.CreatedAt, DateTime.UtcNow);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
