using System;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Friendship
    {
        public Guid Id { get; private set; }
        public Guid RequesterId { get; private set; }
        public Guid FriendId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public FriendState State { get; private set; }

        public Friendship(Guid requesterId, Guid friendId)
        {
            Id = Guid.NewGuid();
            RequesterId = requesterId;
            FriendId = friendId;
            State = FriendState.Requested;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsConfirmed()
        {
            if (State != FriendState.Requested)
                 throw new InvalidFriendshipStateException(Id, State.ToString(), "Requested");
            State = FriendState.Confirmed;
        }
    }

}
