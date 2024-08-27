using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(
                document.Id,
                document.ContextId,
                document.CommentContext,
                document.UserId,
                document.Likes,
                document.ParentId,
                document.TextContent,
                document.CreatedAt,
                document.LastUpdatedAt,
                document.LastReplyAt,
                document.Replies.Select(r => r.AsEntity()),
                document.IsDeleted
            );

        public static CommentDocument ToDocument(this Comment entity)
            => new CommentDocument()
            {
                Id = entity.Id,
                ContextId = entity.ContextId,
                CommentContext = entity.CommentContext,
                UserId = entity.UserId,
                Likes = entity.Likes,
                ParentId = entity.ParentId,
                TextContent = entity.TextContent,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt,
                LastReplyAt = entity.LastReplyAt,
                Replies = entity.Replies.Select(r => r.ToDocument()).ToList(), 
                IsDeleted = entity.IsDeleted
            };

        public static CommentDto AsDto(this CommentDocument document)
            => new CommentDto()
            {
                Id = document.Id,
                ContextId = document.ContextId,
                CommentContext = document.CommentContext.ToString().ToLowerInvariant(),
                UserId = document.UserId,
                Likes = document.Likes,
                ParentId = document.ParentId,
                TextContent = document.TextContent,
                CreatedAt = document.CreatedAt,
                LastUpdatedAt = document.LastUpdatedAt,
                LastReplyAt = document.LastReplyAt,
                RepliesCount = document.Replies.Count(),
                IsDeleted = document.IsDeleted
            };

        public static Reply AsEntity(this ReplyDocument document)
            => new Reply(
                document.Id,
                document.UserId,
                document.CommentId,
                document.TextContent,
                document.CreatedAt
            );

        public static ReplyDocument ToDocument(this Reply entity)  
            => new ReplyDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CommentId = entity.CommentId,
                TextContent = entity.TextContent,
                CreatedAt = entity.CreatedAt
            };
        
        public static CommentDto AsDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                ContextId = comment.ContextId,
                CommentContext = comment.CommentContext.ToString().ToLowerInvariant(),
                UserId = comment.UserId,
                Likes = comment.Likes,
                ParentId = comment.ParentId,
                TextContent = comment.TextContent,
                CreatedAt = comment.CreatedAt,
                LastUpdatedAt = comment.LastUpdatedAt,
                LastReplyAt = comment.LastReplyAt,
                RepliesCount = comment.Replies.Count(),
                IsDeleted = comment.IsDeleted
            };
        }

        public static OrganizationEventCommentDocument ToOrganizationEventDocument(this IEnumerable<Comment> comments, Guid organizationEventId, Guid organizationId)
            => new OrganizationEventCommentDocument
            {
                Id = Guid.NewGuid(),
                OrganizationEventId = organizationEventId,
                OrganizationId = organizationId,
                Comments = comments.Select(c => c.ToDocument()).ToList()  
            };

        public static OrganizationPostCommentDocument ToOrganizationPostDocument(this IEnumerable<Comment> comments, Guid organizationPostId, Guid organizationId)
            => new OrganizationPostCommentDocument
            {
                Id = Guid.NewGuid(),
                OrganizationPostId = organizationPostId,
                OrganizationId = organizationId,
                Comments = comments.Select(c => c.ToDocument()).ToList()  
            };

        public static UserEventCommentDocument ToUserEventDocument(this IEnumerable<Comment> comments, Guid userEventId, Guid userId)
            => new UserEventCommentDocument
            {
                Id = Guid.NewGuid(),
                UserEventId = userEventId,
                UserId = userId,
                Comments = comments.Select(c => c.ToDocument()).ToList()  
            };

        public static UserPostCommentDocument ToUserPostDocument(this IEnumerable<Comment> comments, Guid userPostId, Guid userId)
            => new UserPostCommentDocument
            {
                Id = Guid.NewGuid(),
                UserPostId = userPostId,
                UserId = userId,
                Comments = comments.Select(c => c.ToDocument()).ToList() 
            };
    }
}
