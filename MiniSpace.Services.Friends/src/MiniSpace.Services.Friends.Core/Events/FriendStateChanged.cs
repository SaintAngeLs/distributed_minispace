using System;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class FriendStateChanged : IDomainEvent
    {
        public Guid FriendId { get; }
        public State OldState { get; }
        public State NewState { get; }

        public FriendStateChanged(Friend friend)
        {
            FriendId = friend.FriendId;
            OldState = friend.FriendState; 
            NewState = friend.FriendState;
        }
    }
}
