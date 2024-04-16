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
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(Guid id);
        Task<Tuple<IEnumerable<Event>,int,int,int,int>> BrowseAsync(int pageNumber, int pageSize, string name,
            string organizer, DateTime dateFrom, DateTime dateTo, IEnumerable<string> sortBy, string direction);
    }
}