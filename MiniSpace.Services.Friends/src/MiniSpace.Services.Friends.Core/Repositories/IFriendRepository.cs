using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IFriendRepository
    {
        Task AddFriendAsync(Guid requesterId, Guid friendId);
        Task<List<Friend>> GetFriendsAsync(Guid userId);
        Task<bool> IsFriendAsync(Guid userId, Guid potentialFriendId);
        Task RemoveFriendAsync(Guid requesterId, Guid friendId);
        Task AcceptFriendInvitationAsync(Guid requesterId, Guid friendId);
        Task DeclineFriendInvitationAsync(Guid requesterId, Guid friendId);
        Task<Friend> GetFriendshipAsync(Guid requesterId, Guid friendId);
        Task InviteFriendAsync(Guid inviterId, Guid inviteeId);
        Task UpdateFriendshipAsync(Friend friend);
        Task AddRequestAsync(FriendRequest request); 
        Task UpdateAsync(Friend friend);
        Task AddInvitationAsync(FriendRequest invitation);
        Task AddAsync(Friend friend);
    }
}
