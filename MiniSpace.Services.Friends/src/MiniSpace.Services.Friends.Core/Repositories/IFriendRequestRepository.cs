using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> GetAsync(Guid id);
        Task AddAsync(FriendRequest friendRequest);
        Task UpdateAsync(FriendRequest friendRequest);
        Task DeleteAsync(Guid id);
        Task<FriendRequest> FindByInviterAndInvitee(Guid inviterId, Guid inviteeId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsByUser(Guid userId);
    }
}
