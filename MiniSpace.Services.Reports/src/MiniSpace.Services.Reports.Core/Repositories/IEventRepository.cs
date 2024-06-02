using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Event @event);
        Task DeleteAsync(Guid id);
    }
}