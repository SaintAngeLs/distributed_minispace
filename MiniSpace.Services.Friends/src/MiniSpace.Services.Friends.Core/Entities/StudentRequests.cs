using System;
using System.Collections.Generic;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class StudentRequests : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        private List<FriendRequest> _friendRequests;

        public IEnumerable<FriendRequest> FriendRequests => _friendRequests.AsReadOnly();

        public StudentRequests(Guid studentId)
        {
            Id = Guid.NewGuid(); 
            StudentId = studentId;
            _friendRequests = new List<FriendRequest>();
        }

        public void AddRequest(Guid inviterId, Guid inviteeId, DateTime requestedAt, FriendState state)
        {
            if (state != FriendState.Requested)
                throw new ArgumentException("Initial state must be 'Requested' when adding a new friend request.");

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
                throw new KeyNotFoundException("Friend request not found.");

            _friendRequests.Remove(request);
            AddEvent(new FriendRequestRemoved(request));
        }
    }
}
