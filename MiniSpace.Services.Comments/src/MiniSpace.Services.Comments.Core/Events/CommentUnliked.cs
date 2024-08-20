using System;

namespace MiniSpace.Services.Comments.Core.Events
{
    public class CommentUnliked : IDomainEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }

        public CommentUnliked(Guid commentId, Guid userId)
        {
            CommentId = commentId;
            UserId = userId;
        }
    }
}
