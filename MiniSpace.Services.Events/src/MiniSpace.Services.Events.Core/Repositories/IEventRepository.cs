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
        Task<Tuple<IEnumerable<Event>,int,int,int,int>> BrowseEventsAsync(int pageNumber, int pageSize, string name,
            string organizer, DateTime dateFrom, DateTime dateTo, IEnumerable<string> sortBy, string direction,
            State state, IEnumerable<Guid> eventIds = null);
        Task<Tuple<IEnumerable<Event>,int,int,int,int>> BrowseOrganizerEventsAsync(int pageNumber, int pageSize, string name,
            Guid organizerId, DateTime dateFrom, DateTime dateTo, IEnumerable<string> sortBy, string direction, State? state);
    }
}