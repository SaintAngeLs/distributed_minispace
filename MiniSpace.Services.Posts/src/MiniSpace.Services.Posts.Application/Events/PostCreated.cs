using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostCreated : IEvent
    {
        public Guid PostId { get; }

        public PostCreated(Guid postId)
        {
            PostId = postId;
        }
    }
}
