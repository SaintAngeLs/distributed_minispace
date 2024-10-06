using Paralax.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostDeleted : IEvent
    {
        public Guid PostId { get; }

        public PostDeleted(Guid postId)
        {
            PostId = postId;
        }
    }
}
