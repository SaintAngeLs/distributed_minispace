using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Friends
{
    [Message("friends")]
    public class PendingFriendDeclined : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendDeclined(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
