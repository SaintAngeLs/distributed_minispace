using System;
using Paralax.Types;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class CommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; } 
        public Guid UserId { get; set; } 
        public Guid ParentId { get; set; }
        public string TextContent { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime LastUpdatedAt { get; set; }
        public int RepliesCount { get; set; }
        public bool IsDeleted { get; set; } 
    }
}
