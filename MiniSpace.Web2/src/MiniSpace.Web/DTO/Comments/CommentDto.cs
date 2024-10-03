using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Comments
{
     public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid UserId { get; set; } 
        public IEnumerable<Guid> Likes { get; set; } 
        public Guid ParentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime LastReplyAt { get; set; }
        public int RepliesCount { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ReplyDto> Replies { get; set; }  

        public bool CanExpand { get; set; }
        public HashSet<CommentDto> SubComments { get; set; }
        public int SubCommentsPage { get; set; }
        public CommentDto Parent { get; set; }
        public bool IsLast { get; set; }

    }
    
}
