using Convey.CQRS.Events;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentCreated : IEvent
    {
        public Guid CommentId { get; }

        public CommentCreated(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
