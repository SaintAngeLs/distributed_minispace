using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("posts")]
    public class PostCreated : IEvent
    {
        public Guid PostId { get; }

        public PostCreated(Guid postId)
        {
            PostId = postId;
        }
    }
}