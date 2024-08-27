using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Application.Events.External
{
    [Message("notifications")]
    public class FriendInvited : IEvent, IDomainEvent
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendInvited(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
