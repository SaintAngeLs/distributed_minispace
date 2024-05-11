using Convey.CQRS.Events;
using Convey.MessageBrokers;


namespace MiniSpace.Services.Reactions.Application.Events
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
