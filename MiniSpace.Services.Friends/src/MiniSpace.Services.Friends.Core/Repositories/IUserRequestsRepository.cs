using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IUserRequestsRepository
    {
        Task<UserRequests> GetAsync(Guid userId);
        Task<IEnumerable<UserRequests>> GetAllAsync();
        Task AddAsync(UserRequests userRequests);
        Task UpdateAsync(UserRequests userRequests);
        Task UpdateAsync(Guid userId, IEnumerable<FriendRequest> updatedFriendRequests);
        Task DeleteAsync(Guid userId);
        Task RemoveFriendRequestAsync(Guid requesterId, Guid friendId); 
    }
}
