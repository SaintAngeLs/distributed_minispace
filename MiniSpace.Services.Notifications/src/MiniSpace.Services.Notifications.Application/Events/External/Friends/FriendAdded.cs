using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Friends
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
