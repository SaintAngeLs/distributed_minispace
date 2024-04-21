using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendInvited : IEvent
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
