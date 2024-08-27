using System;
using System.Collections.Generic;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class UserFriends : AggregateRoot
    {
        public Guid UserId { get; private set; }
        private List<Friend> _friends;
        public IEnumerable<Friend> Friends => _friends.AsReadOnly();

        public UserFriends(Guid userId)
        {
            Id = userId; 
            UserId = userId;
            _friends = new List<Friend>();
        }
        public void AddFriend(Friend friend)
        {
            if (_friends.Any(f => f.FriendId == friend.FriendId))
            {
                throw new FriendAlreadyAddedException();
            }
            _friends.Add(friend);
        }

        public void RemoveFriend(Guid friendId)
        {
            var friend = _friends.FirstOrDefault(f => f.FriendId == friendId);
         
            _friends.Remove(friend);
        }

    }
}
