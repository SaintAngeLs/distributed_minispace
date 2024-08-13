using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions
{
    public static class ReactionDocumentExtensions
    {
        public static IEnumerable<Reaction> ToEntities(this UserEventReactionDocument document)
        {
            return document.Reactions.Select(r => r.ToEntity());
        }

        public static UserEventReactionDocument ToDocument(this IEnumerable<Reaction> reactions, Guid userEventId, Guid userId)
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

        public static UserPostReactionDocument ToDocument(this IEnumerable<Reaction> reactions, Guid userPostId, Guid userId)
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

        public static OrganizationPostReactionDocument ToDocument(this IEnumerable<Reaction> reactions, Guid organizationPostId, Guid organizationId)
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

        public static OrganizationEventReactionDocument ToDocument(this IEnumerable<Reaction> reactions, Guid organizationEventId, Guid organizationId)
        {
            return new OrganizationEventReactionDocument
            {
                Id = organizationEventId,
                OrganizationEventId = organizationEventId,
                OrganizationId = organizationId,
                Reactions = reactions.Select(ReactionDocument.FromEntity).ToList()
            };
        }
    }
}
