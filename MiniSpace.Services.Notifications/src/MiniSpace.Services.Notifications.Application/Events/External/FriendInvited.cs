using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Notifications.Core.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Contract]
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
