using System;
using System.Collections.Generic;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class UserRequests : AggregateRoot
    {
        public Guid UserId { get; private set; }
        private List<FriendRequest> _friendRequests;

        public IEnumerable<FriendRequest> FriendRequests => _friendRequests.AsReadOnly();

        public UserRequests(Guid userId)
        {
            Id = Guid.NewGuid(); 
            UserId = userId;
            _friendRequests = new List<FriendRequest>();
        }

        public void AddRequest(Guid inviterId, Guid inviteeId, DateTime requestedAt, FriendState state)
        {
            var friendRequest = new FriendRequest(inviterId, inviteeId, requestedAt, state);
            _friendRequests.Add(friendRequest);
            AddEvent(new FriendRequestCreated(friendRequest));
        }

        public void AcceptRequest(Guid requestId)
        {
            var request = _friendRequests.Find(r => r.Id == requestId);
            if (request == null)
                throw new KeyNotFoundException("Friend request not found.");

            request.Accept();
        }

        public void DeclineRequest(Guid requestId)
        {
            var request = _friendRequests.Find(r => r.Id == requestId);
            if (request == null)
                throw new KeyNotFoundException("Friend request not found.");

            request.Decline();
        }

        public void RemoveRequest(Guid requestId)
        {
            var request = _friendRequests.Find(r => r.Id == requestId);
            if (request == null)
            {
                // // _logger.LogWarning("Attempted to remove a friend request that does not exist: {RequestId}", requestId);
                // return;  // Just return without throwing exception
            }

            _friendRequests.Remove(request);
            AddEvent(new FriendRequestRemoved(request));
        }

        public void UpdateRequestState(Guid requestId, FriendState newState)
        {
            var request = FriendRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.State = newState;
            }
        }
    }
}
