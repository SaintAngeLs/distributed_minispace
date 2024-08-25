using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(User user);
        Task DeleteAsync(Guid id);
    }
}