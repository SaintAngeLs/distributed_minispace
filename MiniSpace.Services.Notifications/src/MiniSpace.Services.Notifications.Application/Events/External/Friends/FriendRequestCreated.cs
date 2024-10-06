using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Friends
{
    public class FriendRequestCreated : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendRequestCreated(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
