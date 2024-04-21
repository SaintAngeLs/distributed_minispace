using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendRequestCreated : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendRequestCreated(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
