using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostUpdated : IEvent
    {
        public Guid PostId { get; }

        public PostUpdated(Guid postId)
        {
            PostId = postId;
        }
    }
}
