using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("notifications")]
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
