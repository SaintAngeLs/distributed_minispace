using System;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Friend : AggregateRoot
    {
        public Guid FriendId { get; private set; }
        public Guid UserId { get; private set; }
        public FriendState FriendState { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Friend(Guid userId, Guid friendId, DateTime createdAt, FriendState state)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            FriendId = friendId;
            CreatedAt = createdAt;
            FriendState = state;
        }

        public static Friend CreateNewFriendship(Guid userId, Guid friendId)
        {
            return new Friend(userId, friendId, DateTime.UtcNow, FriendState.Accepted);
        }

        public void InviteFriend(User inviter, User invitee)
        {
            if (FriendState != FriendState.Unknown)
            {
                throw new InvalidFriendInvitationException(inviter.Id, invitee.Id);
            }
            FriendState = FriendState.Requested;
            Friend newFriend = new Friend(inviter.Id, invitee.Id, DateTime.UtcNow, FriendState.Requested);
            AddEvent(new FriendInvited(this, newFriend));
        }

        public void AcceptFriendship(User friend)
        {
            if (FriendState != FriendState.Requested)
            {
                throw new InvalidFriendStateException(FriendId, "Friendship cannot be accepted in the current state.");
            }
            FriendState = FriendState.Accepted;
            AddEvent(new FriendshipConfirmed(Id));
        }

        public void DeclineFriendship()
        {
            if (FriendState != FriendState.Requested)
                throw new InvalidOperationException("Friendship can only be declined if it is in the requested state.");

            FriendState = FriendState.Declined;
            AddEvent(new FriendshipDeclined(Id, FriendId));
        }


        public void MarkAsConfirmed()
        {
            if (FriendState != FriendState.Requested)
                throw new InvalidFriendshipStateException(Id, FriendState.ToString(), "Requested");

            FriendState = FriendState.Confirmed;
            AddEvent(new FriendshipConfirmed(FriendId));
        }

        public void MarkAsDeclined()
        {
            if (FriendState != FriendState.Requested)
                throw new InvalidFriendshipStateException(Id, FriendState.ToString(), "Only Requested friendships can be declined.");
            
            FriendState = FriendState.Declined;
            AddEvent(new FriendshipDeclined(Id, FriendId));
        }



         public void RemoveFriend(User requester, User friend)
        {
            if (FriendState != FriendState.Accepted)
            {
                throw new InvalidFriendStateException(FriendId, "Only accepted friendships can be removed.");
            }
            FriendState = FriendState.Cancelled;
            
            AddEvent(new FriendRemoved(requester, friend));
        }
    }
}
