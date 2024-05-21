using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public IEnumerable<Guid> Likes { get; set; }
        public Guid ParentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime LastReplyAt { get; set; }
        public int RepliesCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool CanExpand { get; set; }
        public bool IsExpanded { get; set; }
    }    
}
