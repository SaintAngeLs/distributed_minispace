using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Reports.Application.Events.External
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