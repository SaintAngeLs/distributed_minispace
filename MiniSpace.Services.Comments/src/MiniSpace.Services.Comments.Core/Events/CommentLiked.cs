using System;

namespace MiniSpace.Services.Comments.Core.Events
{
    public class CommentLiked : IDomainEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }

        public CommentLiked(Guid commentId, Guid userId)
        {
            CommentId = commentId;
            UserId = userId;
        }
    }
}
