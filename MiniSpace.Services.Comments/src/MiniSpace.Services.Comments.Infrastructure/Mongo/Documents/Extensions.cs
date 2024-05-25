using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(document.Id,document.ContextId,document.CommentContext, document.StudentId, 
                document.StudentName, document.Likes, document.ParentId, document.TextContent, document.CreatedAt,
                document.LastUpdatedAt, document.LastReplyAt, document.RepliesCount, document.IsDeleted);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument()
            {
                Id = entity.Id,
                ContextId = entity.ContextId,
                CommentContext = entity.CommentContext,
                StudentId = entity.StudentId,
                StudentName = entity.StudentName,
                Likes = entity.Likes,
                ParentId = entity.ParentId,
                TextContent = entity.TextContent,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt,
                LastReplyAt = entity.LastReplyAt,
                RepliesCount = entity.RepliesCount,
                IsDeleted = entity.IsDeleted,
            };

        public static CommentDto AsDto(this CommentDocument document)
            => new CommentDto()
            {
                Id = document.Id,
                ContextId = document.ContextId,
                CommentContext = document.CommentContext.ToString().ToLowerInvariant(),
                StudentId = document.StudentId,
                StudentName = document.StudentName,
                Likes = document.Likes,
                ParentId = document.ParentId,
                TextContent = document.TextContent,
                CreatedAt = document.CreatedAt,
                LastUpdatedAt = document.LastUpdatedAt,
                LastReplyAt = document.LastReplyAt,
                RepliesCount = document.RepliesCount,
                IsDeleted= document.IsDeleted,
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument
            {
                Id = entity.Id,
            };
    }    
}
