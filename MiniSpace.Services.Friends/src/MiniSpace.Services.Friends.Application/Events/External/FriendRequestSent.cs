using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Friends.Application.Events.External
{
    [Message("notifications")]
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
