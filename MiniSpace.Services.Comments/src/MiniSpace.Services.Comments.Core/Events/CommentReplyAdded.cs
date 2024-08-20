using System;

namespace MiniSpace.Services.Comments.Core.Events
{
    public class CommentReplyAdded : IDomainEvent
    {
        public Guid CommentId { get; }
        public Guid ReplyId { get; }
        public Guid UserId { get; }
        public string TextContent { get; }
        public DateTime CreatedAt { get; }

        public CommentReplyAdded(Guid commentId, Guid replyId, Guid userId, string textContent, DateTime createdAt)
        {
            CommentId = commentId;
            ReplyId = replyId;
            UserId = userId;
            TextContent = textContent;
            CreatedAt = createdAt;
        }
    }
}
