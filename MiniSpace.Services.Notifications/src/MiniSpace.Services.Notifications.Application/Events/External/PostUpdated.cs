using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("posts")]
    public class PostUpdated : IEvent
    {
        public Guid PostId { get; }

        public PostUpdated(Guid postId)
        {
            PostId = postId;
        }
    }
}