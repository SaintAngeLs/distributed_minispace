using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
   [Contract]
    public class FriendRequestSent : IEvent
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendRequestSent(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
