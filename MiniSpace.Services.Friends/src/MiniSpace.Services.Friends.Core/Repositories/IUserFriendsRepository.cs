using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IUserFriendsRepository
    {
        Task<UserFriends> GetAsync(Guid userId);
        Task<IEnumerable<UserFriends>> GetAllAsync();
        Task AddAsync(UserFriends userFriends);
        Task UpdateAsync(UserFriends userFriends);
        Task DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        Task<IEnumerable<Friend>> GetFriendsAsync(Guid userId);
        Task AddOrUpdateAsync(UserFriends userFriends);
        Task RemoveFriendAsync(Guid userId, Guid friendId);
    }
}
