using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Posts.Application.Events.External
{
    [Message("comments")]
    public class CommentCreated : IEvent
    {
        public Guid CommentId { get; }
        public Guid ContextId { get; }
        public string CommentContext { get; }
        public Guid UserId { get; }
        public Guid ParentId { get; }
        public string TextContent { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
        public int RepliesCount { get; }
        public bool IsDeleted { get; }

        public CommentCreated(Guid commentId, Guid contextId, string commentContext, Guid userId, 
                              Guid parentId, string textContent, DateTime createdAt, 
                              DateTime lastUpdatedAt, int repliesCount, bool isDeleted)
        {
            CommentId = commentId;
            ContextId = contextId;
            CommentContext = commentContext;
            UserId = userId;
            ParentId = parentId;
            TextContent = textContent;
            CreatedAt = createdAt;
            LastUpdatedAt = lastUpdatedAt;
            RepliesCount = repliesCount;
            IsDeleted = isDeleted;
        }
    }
}
