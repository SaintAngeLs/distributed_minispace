using System;
using System.Threading.Tasks;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetByResetTokenAsync(string resetToken);
    }
}