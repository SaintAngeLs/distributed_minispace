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

        public void AddFriend(Friend friend)
        {
            _friends.Add(friend);
        }

    }
}
