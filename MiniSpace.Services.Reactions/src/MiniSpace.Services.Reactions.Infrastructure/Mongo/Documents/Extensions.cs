using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions
{
    public static class Extensions
    {
        public static ReactionDocument AsDocument(this Reaction reaction)
        {
            return new ReactionDocument
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                ReactionType = reaction.ReactionType,
                ContentId = reaction.ContentId,
                ContentType = reaction.ContentType,
                TargetType = reaction.TargetType
            };
        }

        public static IEnumerable<Reaction> ToEntities(this UserEventReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
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

        public static UserEventReactionDocument ToUserEventDocument(this IEnumerable<Reaction> reactions, Guid userEventId, Guid userId)
        {
            return new UserEventReactionDocument
            {
                Id = userEventId,
                UserEventId = userEventId,
                UserId = userId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this UserPostReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static ReactionDto AsDto(this ReactionDocument document)
        {
            return new ReactionDto
            {
                Id = document.Id,
                UserId = document.UserId,
                ContentId = document.ContentId,
                ContentType = document.ContentType,
                ReactionType = document.ReactionType,
                TargetType = document.TargetType
            };
        }

        public static UserPostReactionDocument ToUserPostDocument(this IEnumerable<Reaction> reactions, Guid userPostId, Guid userId)
        {
            return new UserPostReactionDocument
            {
                Id = userPostId,
                UserPostId = userPostId,
                UserId = userId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this OrganizationPostReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static OrganizationPostReactionDocument ToOrganizationPostDocument(this IEnumerable<Reaction> reactions, Guid organizationPostId, Guid organizationId)
        {
            return new OrganizationPostReactionDocument
            {
                Id = organizationPostId,
                OrganizationPostId = organizationPostId,
                OrganizationId = organizationId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this OrganizationEventReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static OrganizationEventReactionDocument ToOrganizationEventDocument(this IEnumerable<Reaction> reactions, Guid organizationEventId, Guid organizationId)
        {
            return new OrganizationEventReactionDocument
            {
                Id = organizationEventId,
                OrganizationEventId = organizationEventId,
                OrganizationId = organizationId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this OrganizationEventCommentsReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static OrganizationPostCommentsReactionDocument ToOrganizationPostCommentDocument(this IEnumerable<Reaction> reactions, Guid organizationPostCommentId, Guid organizationId)
        {
            return new OrganizationPostCommentsReactionDocument
            {
                Id = organizationPostCommentId,
                OrganizationPostCommentId = organizationPostCommentId,
                OrganizationId = organizationId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this OrganizationPostCommentsReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static UserEventCommentsReactionDocument ToUserEventCommentDocument(this IEnumerable<Reaction> reactions, Guid userEventCommentId, Guid userId)
        {
            return new UserEventCommentsReactionDocument
            {
                Id = userEventCommentId,
                UserEventCommentId = userEventCommentId,
                UserId = userId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static IEnumerable<Reaction> ToEntities(this UserEventCommentsReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static OrganizationEventCommentsReactionDocument ToOrganizationEventCommentDocument(this IEnumerable<Reaction> reactions, Guid organizationEventCommentId, Guid organizationId)
        {
            return new OrganizationEventCommentsReactionDocument
            {
                Id = organizationEventCommentId,
                OrganizationEventCommentId = organizationEventCommentId,
                OrganizationId = organizationId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }

        public static UserPostCommentsReactionDocument ToUserPostCommentDocument(this IEnumerable<Reaction> reactions, Guid userPostCommentId, Guid userId)
        {
            return new UserPostCommentsReactionDocument
            {
                Id = userPostCommentId,
                UserPostCommentId = userPostCommentId,
                UserId = userId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }
        public static IEnumerable<Reaction> ToEntities(this UserPostCommentsReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

           public static ReactionDto AsDto(this Reaction reaction)
        {
            return new ReactionDto
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                ContentId = reaction.ContentId,
                ContentType = reaction.ContentType,
                ReactionType = reaction.ReactionType,
                TargetType = reaction.TargetType
            };
        }

        // public static ReactionDto AsDto(this ReactionDocument document)
        // {
        //     return new ReactionDto
        //     {
        //         Id = document.Id,
        //         UserId = document.UserId,
        //         ContentId = document.ContentId,
        //         ContentType = document.ContentType,
        //         ReactionType = document.ReactionType,
        //         TargetType = document.TargetType
        //     };
        // }


        public static UpdateDefinition<TDocument> Push<TDocument, TItem>(
            this UpdateDefinitionBuilder<TDocument> builder,
            Expression<Func<TDocument, IEnumerable<TItem>>> field,
            TItem value)
        {
            return builder.AddToSet(field, value);
        }

        public static UpdateDefinition<TDocument> Set<TDocument, TField>(
            this UpdateDefinitionBuilder<TDocument> builder,
            Expression<Func<TDocument, TField>> field,
            TField value)
        {
            return builder.Set(field, value);
        }

        public static UpdateDefinition<TDocument> PullFilter<TDocument, TItem>(
            this UpdateDefinitionBuilder<TDocument> builder,
            Expression<Func<TDocument, IEnumerable<TItem>>> field,
            Expression<Func<TItem, bool>> filter)
        {
            return builder.PullFilter(field, filter);
        }
    }
}
