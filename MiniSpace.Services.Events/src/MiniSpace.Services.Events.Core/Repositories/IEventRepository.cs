using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Application.Wrappers;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid id);
        Task<IEnumerable<Event>> GetAllAsync();
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<(IEnumerable<Event> events, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseEventsAsync(
            int pageNumber, int pageSize, string name, string organizer, DateTime dateFrom, DateTime dateTo, 
            Category? category, State? state, IEnumerable<Guid> organizations, IEnumerable<Guid> friends, 
            EventEngagementType? friendsEngagementType, IEnumerable<string> sortBy, string direction);
        Task<(IEnumerable<Event> events, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseOrganizerEventsAsync(
            int pageNumber, int pageSize, string name, Guid organizerId, DateTime dateFrom, DateTime dateTo,
            IEnumerable<string> sortBy, string direction, State? state);
        Task<(IEnumerable<Event> events, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseStudentEventsAsync(
            int pageNumber, int pageSize, IEnumerable<Guid> eventIds, IEnumerable<string> sortBy, string direction);
        Task<IEnumerable<Event>> GetEventsToBePublishedAsync(DateTime now);
        Task<IEnumerable<Event>> GetEventsToArchiveAsync(DateTime now);
    }
}