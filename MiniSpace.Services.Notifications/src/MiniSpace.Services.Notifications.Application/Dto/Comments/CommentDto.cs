using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Notifications.Application.Dto.Comments;

namespace MiniSpace.Services.Notifications.Application.Dto.Comments
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

        public CommentDto()
        {
        }
    }
}
