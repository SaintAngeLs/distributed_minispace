using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class EventMongoRepository : IEventRepository
    {
        private readonly IMongoRepository<EventDocument, Guid> _repository;

        public EventMongoRepository(IMongoRepository<EventDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(Event @event)
            => _repository.AddAsync(@event.AsDocument());

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(s => s.Id == id);
    }    
}
