using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IEventsUserViewsRepository
    {
        Task<EventsViews> GetAsync(Guid userId);
        Task AddAsync(EventsViews eventsViews);
        Task UpdateAsync(EventsViews eventsViews);
        Task DeleteAsync(Guid userId);
    }
}
