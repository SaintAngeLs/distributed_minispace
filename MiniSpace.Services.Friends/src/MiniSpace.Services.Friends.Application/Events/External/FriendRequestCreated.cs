using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Friends.Application.Events.External
{
    [Message("notifications")]
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
