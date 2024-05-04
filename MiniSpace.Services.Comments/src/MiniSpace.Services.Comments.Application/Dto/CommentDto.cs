using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Comments.Application.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public Guid StudentId { get; set; }
        public List<Guid> Likes { get; set; }
        public Guid ParentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}