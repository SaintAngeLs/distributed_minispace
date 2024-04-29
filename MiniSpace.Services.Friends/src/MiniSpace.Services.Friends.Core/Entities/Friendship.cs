using System;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Friendship : AggregateRoot
    {
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
            AddEvent(new FriendshipConfirmed(Id));
        }

        public void DeclineFriendship()
        {
            if (State != FriendState.Requested)
                throw new InvalidOperationException("Friendship can only be declined if it is in the requested state.");

            State = FriendState.Declined;
            // Assuming 'Id' is the ID of the current object and 'FriendId' is the ID of the friend.
            AddEvent(new FriendshipDeclined(RequesterId, FriendId));
        }


    }

}
