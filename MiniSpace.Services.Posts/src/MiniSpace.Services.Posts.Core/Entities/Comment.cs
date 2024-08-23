using System;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        
        public Comment(Guid id, Guid postId, Guid userId, string textContent, DateTime createdAt)
        {
            Id = id;
            PostId = postId;
            UserId = userId;
            TextContent = textContent;
            CreatedAt = createdAt;
            LastUpdatedAt = createdAt;
        }

        public void UpdateText(string newText, DateTime updatedAt)
        {
            TextContent = newText;
            LastUpdatedAt = updatedAt;
        }
    }
}
