using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Comments.Application.Events
{
    public class CommentUpdated : IEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }
        public DateTime UpdatedAt { get; }
        public string CommentContent { get; }

        public CommentUpdated(Guid commentId, Guid userId, string commentContext, DateTime updatedAt, string commentContent)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
            UpdatedAt = updatedAt;
            CommentContent = commentContent;
        }
    }
}
