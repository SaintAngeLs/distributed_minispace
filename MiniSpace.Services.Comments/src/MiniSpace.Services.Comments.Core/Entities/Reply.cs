using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Reply
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid CommentId { get; }
        public string TextContent { get; }
        public DateTime CreatedAt { get; }

        public Reply(Guid id, Guid userId, Guid commentId, string textContent, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            CommentId = commentId;
            TextContent = textContent;
            CreatedAt = createdAt;
        }
    }
}