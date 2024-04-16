using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

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
            var @event = await _repository.GetAsync(o => o.Id == id);

            return @event?.AsEntity();
        }

        public async Task<> BrowseAsync(int pageNumber, int pageSize, string name, string organizer, 
            DateTime dateFrom, DateTime dateTo, SortDto sortDto)
        {
            var filterDefinition = Repositories.Extensions.ToFilterDefinition(name, organizer, dateFrom, dateTo);
            var sortDefinition = sortDto.ToSortDefinition();
            
            var pagedEvents = await _repository.Collection.AggregateByPage(
                filterDefinition,
                sortDefinition,
                pageNumber,
                pageSize);
            
            return pagedEvents;
        }

        public Task AddAsync(Event @event) => _repository.AddAsync(@event.AsDocument());
        public Task UpdateAsync(Event @event) => _repository.UpdateAsync(@event.AsDocument());
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}