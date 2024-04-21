using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class PendingFriendAccepted : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendAccepted(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
