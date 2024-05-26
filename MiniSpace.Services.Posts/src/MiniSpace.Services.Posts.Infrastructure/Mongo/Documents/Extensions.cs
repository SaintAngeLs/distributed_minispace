using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Post AsEntity(this PostDocument document)
            => new Post(document.Id, document.EventId, document.OrganizerId, document.TextContent,
                document.MediaContent, document.CreatedAt, document.State, document.PublishDate, document.UpdatedAt);

        public static PostDocument AsDocument(this Post entity)
            => new PostDocument()
            {
                Id = entity.Id,
                EventId = entity.EventId,
                OrganizerId = entity.OrganizerId,
                TextContent = entity.TextContent,
                MediaContent = entity.MediaContent,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                State = entity.State,
                PublishDate = entity.PublishDate
            };

        public static PostDto AsDto(this PostDocument document)
            => new PostDto()
            {
                Id = document.Id,
                EventId = document.EventId,
                OrganizerId = document.OrganizerId,
                TextContent = document.TextContent,
                MediaContent = document.MediaContent,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                State = document.State.ToString().ToLowerInvariant(),
                PublishDate = document.PublishDate
            };
        
        public static Event AsEntity(this EventDocument document)
            => new Event(document.Id, document.OrganizerId);

        public static EventDocument AsDocument(this Event entity)
            => new EventDocument
            {
                Id = entity.Id,
                OrganizerId = entity.OrganizerId
            };
    }    
}
