using System;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid id);
        Task AddAsync(Event activity);
        Task UpdateAsync(Event activity);
        Task DeleteAsync(Guid id);
    }
}