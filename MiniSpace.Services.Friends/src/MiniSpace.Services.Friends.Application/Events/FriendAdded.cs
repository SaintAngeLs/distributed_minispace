using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendAdded : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendAdded(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
