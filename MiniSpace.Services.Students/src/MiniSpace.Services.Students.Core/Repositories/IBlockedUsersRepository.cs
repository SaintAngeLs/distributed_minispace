using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IBlockedUsersRepository
    {
        Task<BlockedUser> GetAsync(Guid blockerId, Guid blockedUserId);
        Task AddAsync(BlockedUser blockedUser);
        Task UpdateAsync(BlockedUser blockedUser);
        Task DeleteAsync(Guid blockerId, Guid blockedUserId);
    }
}
