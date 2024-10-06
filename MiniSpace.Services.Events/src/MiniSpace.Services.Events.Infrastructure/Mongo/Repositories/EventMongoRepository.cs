using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
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

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var events = _repository.Collection.AsQueryable();
            var filteredEvents = await events.Where(e 
                => e.State == State.ToBePublished || e.State == State.Published).ToListAsync();
            return filteredEvents.Select(e => e.AsEntity());
        }

        private async Task<(int totalPages, int totalElements, IEnumerable<EventDocument> data)> BrowseAsync(
            FilterDefinition<EventDocument> filterDefinition, SortDefinition<EventDocument> sortDefinition, 
            int pageNumber, int pageSize)
        {
            var pagedEvents = await _repository.Collection.AggregateByPage(
                filterDefinition,
                sortDefinition,
                pageNumber,
                pageSize);

            return pagedEvents;
        }
        
        public async Task<(IEnumerable<Event> events, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseEventsAsync(
            int pageNumber, int pageSize, string name, string organizer, DateTime dateFrom, DateTime dateTo, 
            Category? category, State? state, IEnumerable<Guid> organizations, IEnumerable<Guid> friends, 
            EventEngagementType? friendsEngagementType, IEnumerable<string> sortBy, string direction)
        {
            var filterDefinition = Extensions.ToFilterDefinition(name, dateFrom, dateTo)
                .AddOrganizerNameFilter(organizer)
                .AddCategoryFilter(category)
                .AddRestrictedStateFilter(state)
                .AddFriendsFilter(friends, friendsEngagementType)
                .AddOrganizationsIdFilter(organizations); 

                
            
            var sortDefinition = Extensions.ToSortDefinition(sortBy, direction);
            
            var pagedEvents = await BrowseAsync(filterDefinition, sortDefinition, pageNumber, pageSize);

            return (pagedEvents.data.Select(e => e.AsEntity()), pageNumber, pageSize,
                pagedEvents.totalPages, pagedEvents.totalElements);
        }
        
        public async Task<(IEnumerable<Event> events, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseOrganizerEventsAsync(int pageNumber, 
            int pageSize, string name, Guid organizerId, DateTime dateFrom, DateTime dateTo, IEnumerable<string> sortBy, 
            string direction, State? state)
        {
            var filterDefinition = Extensions.ToFilterDefinition(name, dateFrom, dateTo)
                .AddOrganizerIdFilter(organizerId)
                .AddStateFilter(state);
            var sortDefinition = Extensions.ToSortDefinition(sortBy, direction);
            
            var pagedEvents = await BrowseAsync(filterDefinition, sortDefinition, pageNumber, pageSize);
            
            return (pagedEvents.data.Select(e => e.AsEntity()), pageNumber, pageSize, 
                pagedEvents.totalPages, pagedEvents.totalElements);
        }

        public async Task<(IEnumerable<Event> events, int pageNumber, int pageSize, int totalPages, int totalElements)> BrowseStudentEventsAsync(
                int pageNumber, int pageSize, IEnumerable<Guid> eventIds, IEnumerable<string> sortBy, string direction)
        {
            var filterDefinition = Extensions.CreateFilterDefinition()
                .AddEventIdFilter(eventIds);
            
            var sortDefinition = Extensions.ToSortDefinition(sortBy, direction);
            
            var pagedEvents = await BrowseAsync(filterDefinition, sortDefinition, pageNumber, pageSize);
            
            return (pagedEvents.data.Select(e => e.AsEntity()), pageNumber, pageSize, 
                pagedEvents.totalPages, pagedEvents.totalElements);
        }

         public async Task<IEnumerable<Event>> GetEventsToBePublishedAsync(DateTime now)
        {
            var events = await _repository.Collection.AsQueryable()
                .Where(e => e.State == State.ToBePublished && e.PublishDate <= now)
                .ToListAsync();

            return events.Select(e => e.AsEntity());
        }

        public async Task<IEnumerable<Event>> GetEventsToArchiveAsync(DateTime now)
        {
            var events = await _repository.Collection.AsQueryable()
                .Where(e => e.State == State.Published && e.EndDate <= now)
                .ToListAsync();

            return events.Select(e => e.AsEntity());
        }

        public Task AddAsync(Event @event) => _repository.AddAsync(@event.AsDocument());
        public Task UpdateAsync(Event @event) => _repository.UpdateAsync(@event.AsDocument());
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(e => e.Id == id);
    }
}