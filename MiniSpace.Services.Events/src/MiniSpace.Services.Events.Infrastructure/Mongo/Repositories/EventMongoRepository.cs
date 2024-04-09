using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Repositories
{
    public class EventMongoRepository : IEventRepository
    {
        private readonly IMongoRepository<EventDocument, Guid> _repository;

        public EventMongoRepository(IMongoRepository<EventDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Event> GetAsync(Guid id)
        {
            var activity = await _repository.GetAsync(o => o.Id == id);

            return activity?.AsEntity();
        }

        public Task AddAsync(Event activity) => _repository.AddAsync(activity.AsDocument());
        public Task UpdateAsync(Event activity) => _repository.UpdateAsync(activity.AsDocument());
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}