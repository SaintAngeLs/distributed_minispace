using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Post AsEntity(this PostDocument document)
            => new Post(document.Id, document.EventId, document.StudentId, document.TextContent,
                document.MediaContent, document.CreatedAt, document.State, document.PublishDate);

        public static PostDocument AsDocument(this Post entity)
            => new PostDocument()
            {
                Id = entity.Id,
                EventId = entity.EventId,
                StudentId = entity.StudentId,
                TextContent = entity.TextContent,
                MediaContent = entity.MediaContent,
                CreatedAt = entity.CreatedAt,
                State = entity.State,
                PublishDate = entity.PublishDate
            };

        public static PostDto AsDto(this PostDocument document)
            => new PostDto()
            {
                Id = document.Id,
                EventId = document.EventId,
                StudentId = document.StudentId,
                TextContent = document.TextContent,
                MediaContent = document.MediaContent,
                CreatedAt = document.CreatedAt,
                State = document.State.ToString().ToLowerInvariant(),
                PublishDate = document.PublishDate
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
