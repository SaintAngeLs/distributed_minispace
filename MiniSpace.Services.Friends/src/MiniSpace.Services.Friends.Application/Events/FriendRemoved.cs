using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendRemoved : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendRemoved(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
