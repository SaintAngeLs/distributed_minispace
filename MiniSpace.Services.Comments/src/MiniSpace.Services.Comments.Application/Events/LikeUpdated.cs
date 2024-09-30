using Paralax.CQRS.Events;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class LikeUpdated : IEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public string CommentContext { get; }
        public DateTime UpdatedAt { get; }
        public string CommentContent { get; }

        public LikeUpdated(Guid commentId, Guid userId, string commentContext, DateTime updatedAt, string commentContent)
        {
            CommentId = commentId;
            UserId = userId;
            CommentContext = commentContext;
            UpdatedAt = updatedAt;
            CommentContent = commentContent;
        }
    }
}
