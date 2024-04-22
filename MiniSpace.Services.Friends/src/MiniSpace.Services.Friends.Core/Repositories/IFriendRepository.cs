using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IFriendRepository
    {
        Task AddFriendAsync(Guid requesterId, Guid friendId);
        Task<List<Friend>> GetFriendsAsync(Guid studentId);
        Task<bool> IsFriendAsync(Guid studentId, Guid potentialFriendId);
        Task RemoveFriendAsync(Guid requesterId, Guid friendId);
        Task AcceptFriendInvitationAsync(Guid requesterId, Guid friendId);
        Task DeclineFriendInvitationAsync(Guid requesterId, Guid friendId);
        Task<Friend> GetFriendshipAsync(Guid requesterId, Guid friendId);
        Task UpdateFriendshipAsync(Friend friend);
    }
}
