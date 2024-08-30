using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }                  
        public Guid ContextId { get; private set; }           
        public string CommentContext { get; private set; }    
        public Guid UserId { get; private set; }              
        public Guid ParentId { get; private set; }            
        public string TextContent { get; private set; }       
        public DateTime CreatedAt { get; private set; }       
        public DateTime LastUpdatedAt { get; private set; }   
        public int RepliesCount { get; private set; }         
        public bool IsDeleted { get; private set; }           

        public Comment(Guid id, Guid contextId, string commentContext, Guid userId, 
                       Guid parentId, string textContent, DateTime createdAt, 
                       DateTime lastUpdatedAt, int repliesCount, bool isDeleted)
        {
            Id = id;
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

        public void UpdateText(string newText, DateTime updatedAt)
        {
            TextContent = newText;
            LastUpdatedAt = updatedAt;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            TextContent = "[deleted]";
        }

        public void IncrementRepliesCount()
        {
            RepliesCount++;
        }

        public void DecrementRepliesCount()
        {
            if (RepliesCount > 0)
            {
                RepliesCount--;
            }
        }
    }
}
