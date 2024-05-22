using System;
using System.Collections.Generic;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class StudentFriends : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        private List<Friend> _friends;
        public IEnumerable<Friend> Friends => _friends.AsReadOnly();

        public StudentFriends(Guid studentId)
        {
            Id = studentId; 
            StudentId = studentId;
            _friends = new List<Friend>();
        }

        // public void AddFriend(Friend friend)
        // {
        //     _friends.Add(friend);
        // }

        public void AddFriend(Friend friend)
        {
            if (_friends.Any(f => f.FriendId == friend.FriendId))
            {
                throw new InvalidOperationException("This friend is already added.");
            }
            _friends.Add(friend);
        }

        public void RemoveFriend(Guid friendId)
        {
            var friend = _friends.FirstOrDefault(f => f.FriendId == friendId);
            // if (friend == null)
            // {
            //     throw new InvalidOperationException("Friend not found.");
            // }
            _friends.Remove(friend);
        }

    }
}
