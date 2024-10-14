using Paralax.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events.Rejected
{
    public class FriendInvitationFailed : IRejectedEvent
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }
        public string Reason { get; }
        public string Code { get; }

        public FriendInvitationFailed(Guid inviterId, Guid inviteeId, string reason, string code)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
            Reason = reason;
            Code = code;
        }
    }

}
