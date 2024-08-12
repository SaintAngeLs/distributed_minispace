using System;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionsUserEventRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task<Event> GetByIdAsync(Guid id); 
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event); 
        Task DeleteAsync(Guid id); 
    }
}
