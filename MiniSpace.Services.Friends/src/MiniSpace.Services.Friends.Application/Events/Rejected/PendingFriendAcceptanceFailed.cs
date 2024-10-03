using Paralax.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events.Rejected
{
    public class PendingFriendAcceptanceFailed : IRejectedEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public string Reason { get; }
        public string Code { get; }

        public PendingFriendAcceptanceFailed(Guid requesterId, Guid friendId, string reason, string code)
        {
            RequesterId = requesterId;
            FriendId = friendId;
            Reason = reason;
            Code = code;
        }
    }
}
