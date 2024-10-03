using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendRequestWithdrawn : IEvent
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendRequestWithdrawn(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
