using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IBlockedUsersRepository
    {
        Task<BlockedUsers> GetAsync(Guid userId);
        Task AddAsync(BlockedUsers blockedUsers);
        Task UpdateAsync(BlockedUsers blockedUsers);
        Task DeleteAsync(Guid userId);
    }
}
