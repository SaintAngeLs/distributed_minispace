using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendshipConfirmed : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendshipConfirmed(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
