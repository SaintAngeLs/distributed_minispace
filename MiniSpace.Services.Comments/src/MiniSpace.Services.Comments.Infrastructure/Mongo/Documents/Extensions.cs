using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Comment AsEntity(this CommentDocument document)
            => new Comment(document.Id,document.ContextId,document.CommentContext, document.StudentId, document.Likes, document.ParentId,
                document.TextContent, document.CreatedAt);

        public static CommentDocument AsDocument(this Comment entity)
            => new CommentDocument()
            {
                Id = entity.Id,
                ContextId = entity.ContextId,
                CommentContext = entity.CommentContext,
                StudentId = entity.StudentId,
                Likes = entity.Likes,
                ParentId = entity.ParentId,
                TextContent = entity.TextContent,
                CreatedAt = entity.CreatedAt,
            };

        public static CommentDto AsDto(this CommentDocument document)
            => new CommentDto()
            {
                Id = document.Id,
                ContextId = document.ContextId,
                CommentContext = document.CommentContext.ToString().ToLowerInvariant(),
                StudentId = document.StudentId,
                Likes = document.Likes,
                ParentId = document.ParentId,
                TextContent = document.TextContent,
                CreatedAt = document.CreatedAt
            };
        
        public static Student AsEntity(this StudentDocument document)
            => new Student(document.Id, document.FullName);

        public static StudentDocument AsDocument(this Student entity)
            => new StudentDocument
            {
                Id = entity.Id,
                FullName = entity.FullName
            };
    }    
}
