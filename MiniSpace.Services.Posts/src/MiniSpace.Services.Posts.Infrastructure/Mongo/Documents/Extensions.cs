using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Post AsEntity(this PostDocument document)
        => new Post(
            document.Id,
            document.UserId,
            document.OrganizationId,
            document.EventId,
            document.TextContent,
            document.MediaFiles,
            document.CreatedAt,
            document.State,
            document.Context,
            document.PublishDate,
            document.Visibility,  
            document.UpdatedAt);

        public static PostDocument AsDocument(this Post entity)
            => new PostDocument()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                OrganizationId = entity.OrganizationId,
                EventId = entity.EventId,
                TextContent = entity.TextContent,
                MediaFiles = entity.MediaFiles,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                State = entity.State,
                PublishDate = entity.PublishDate,
                Context = entity.Context,
                Visibility = entity.Visibility 
            };

        public static PostDto AsDto(this PostDocument document)
            => new PostDto()
            {
                Id = document.Id,
                UserId = document.UserId,
                OrganizationId = document.OrganizationId,
                EventId = document.EventId,
                TextContent = document.TextContent,
                MediaFiles = document.MediaFiles,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                State = document.State.ToString().ToLowerInvariant(),
                PublishDate = document.PublishDate,
                Visibility = document.Visibility.ToString().ToLowerInvariant() 
            };

        public static PostDto AsDto(this Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                OrganizationId = post.OrganizationId,
                EventId = post.EventId,
                TextContent = post.TextContent,
                MediaFiles = post.MediaFiles,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                State = post.State.ToString().ToLowerInvariant(),
                PublishDate = post.PublishDate,
                Visibility = post.Visibility.ToString().ToLowerInvariant()
            };
        }
    }
}
