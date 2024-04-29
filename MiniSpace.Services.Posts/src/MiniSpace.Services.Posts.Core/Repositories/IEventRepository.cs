using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Event @event);
        Task DeleteAsync(Guid id);
    }
}
