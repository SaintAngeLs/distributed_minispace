using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Application.Dto
{
    [ExcludeFromCodeCoverage]
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
        public IEnumerable<ReplyDto> Replies { get; set; } // Including replies in the DTO

        public CommentDto()
        {
        }

        public CommentDto(Comment comment)
        {
            Id = comment.Id;
            ContextId = comment.ContextId;
            CommentContext = comment.CommentContext.ToString().ToLowerInvariant();
            UserId = comment.UserId;
            Likes = comment.Likes;
            ParentId = comment.ParentId;
            TextContent = comment.TextContent;
            CreatedAt = comment.CreatedAt;
            LastUpdatedAt = comment.LastUpdatedAt;
            LastReplyAt = comment.LastReplyAt;
            RepliesCount = comment.RepliesCount;
            IsDeleted = comment.IsDeleted;
            Replies = comment.Replies.Select(reply => new ReplyDto(reply)).ToList();
        }
    }
}
