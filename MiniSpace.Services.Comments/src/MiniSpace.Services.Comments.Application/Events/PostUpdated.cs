using Convey.CQRS.Events;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }

        public CommentUpdated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
