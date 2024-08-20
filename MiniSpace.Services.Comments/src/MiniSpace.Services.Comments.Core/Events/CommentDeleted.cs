using System;

namespace MiniSpace.Services.Comments.Core.Events
{
    public class CommentDeleted : IDomainEvent
    {
        public Guid CommentId { get; }

        public CommentDeleted(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
