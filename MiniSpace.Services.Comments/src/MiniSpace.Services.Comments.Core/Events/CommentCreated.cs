using System;

namespace MiniSpace.Services.Comments.Core.Events
{
    public class CommentCreated : IDomainEvent
    {
        public Guid CommentId { get; }
        public Guid UserId { get; }
        public Guid ContextId { get; }
        public string TextContent { get; }
        public DateTime CreatedAt { get; }

        public CommentCreated(Guid commentId, Guid userId, Guid contextId, string textContent, DateTime createdAt)
        {
            CommentId = commentId;
            UserId = userId;
            ContextId = contextId;
            TextContent = textContent;
            CreatedAt = createdAt;
        }
    }
}
