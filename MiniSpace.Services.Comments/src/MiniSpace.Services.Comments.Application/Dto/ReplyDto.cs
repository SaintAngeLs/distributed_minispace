using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReplyDto(Reply reply)
        {
            Id = reply.Id;
            UserId = reply.UserId;
            CommentId = reply.CommentId;
            TextContent = reply.TextContent;
            CreatedAt = reply.CreatedAt;
        }
    }
}
