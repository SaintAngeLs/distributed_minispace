using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<List<User>> GetUsersByEventIdAsync(Guid eventId);
        Task AddAsync(User student);
        Task UpdateAsync(User student);
        Task DeleteAsync(Guid id);
    }
}
