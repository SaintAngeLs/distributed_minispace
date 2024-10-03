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
                document.Type,
                document.Title,
                document.Visibility,
                document.UpdatedAt,
                document.PageOwnerId,
                document.PageOwnerType,
                document.OriginalPostId);

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
                Visibility = entity.Visibility,
                Type = entity.Type,
                Title = entity.Title,
                PageOwnerId = entity.PageOwnerId,
                PageOwnerType = entity.PageOwnerType,
                OriginalPostId = entity.OriginalPostId
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
                Visibility = document.Visibility.ToString().ToLowerInvariant(),
                PageOwnerId = document.PageOwnerId,
                PageOwnerType = document.PageOwnerType.ToString().ToLowerInvariant(),
                OriginalPostId = document.OriginalPostId
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
                Visibility = post.Visibility.ToString().ToLowerInvariant(),
                PageOwnerId = post.PageOwnerId,
                PageOwnerType = post.PageOwnerType.ToString().ToLowerInvariant(),
                OriginalPostId = post.OriginalPostId
            };
        }

        public static CommentDocument AsDocument(this Comment comment)
        {
            return new CommentDocument
            {
                Id = comment.Id,
                ContextId = comment.ContextId,
                CommentContext = comment.CommentContext,
                UserId = comment.UserId,
                ParentId = comment.ParentId,
                TextContent = comment.TextContent,
                CreatedAt = comment.CreatedAt,
                LastUpdatedAt = comment.LastUpdatedAt,
                RepliesCount = comment.RepliesCount,
                IsDeleted = comment.IsDeleted
            };
        }

        public static Comment AsEntity(this CommentDocument document)
        {
            return new Comment(
                document.Id,
                document.ContextId,
                document.CommentContext,
                document.UserId,
                document.ParentId,
                document.TextContent,
                document.CreatedAt,
                document.LastUpdatedAt,
                document.RepliesCount,
                document.IsDeleted
            );
        }

        public static UserCommentsDocument AsDocument(this IEnumerable<Comment> comments, Guid userId)
        {
            return new UserCommentsDocument
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Comments = comments.Select(comment => comment.AsDocument()).ToList()
            };
        }

        public static IEnumerable<Comment> AsEntities(this UserCommentsDocument document)
        {
            return document.Comments.Select(doc => doc.AsEntity());
        }

        public static ReactionDocument AsDocument(this Reaction reaction)
        {
            return new ReactionDocument
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                ContentId = reaction.ContentId,
                ContentType = reaction.ContentType,
                ReactionType = reaction.Type,
                TargetType = reaction.TargetType,
                CreatedAt = reaction.CreatedAt
            };
        }

         public static Reaction AsEntity(this ReactionDocument document)
        {
            return Reaction.Create(
                document.Id,
                document.UserId,
                document.ReactionType,
                document.ContentId,
                document.ContentType,
                document.TargetType
            );
        }

        public static UserReactionDocument AsDocument(this IEnumerable<Reaction> reactions, Guid userId)
        {
            return new UserReactionDocument
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Reactions = reactions.Select(reaction => reaction.AsDocument()).ToList()
            };
        }

        public static IEnumerable<Reaction> AsEntities(this UserReactionDocument document)
        {
            return document.Reactions.Select(doc => doc.AsEntity());
        }
    }
}
