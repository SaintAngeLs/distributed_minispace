using System;
using Paralax.Types;
using MiniSpace.Services.Reactions.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class ReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public Guid ContentId { get; set; }
        public ReactionContentType ContentType { get; set; }
        public ReactionTargetType TargetType { get; set; }

        public static ReactionDocument FromEntity(Reaction reaction)
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

        public Reaction ToEntity()
        {
            return Reaction.Create(Id, UserId, ReactionType, ContentId, ContentType, TargetType);
        }
    }
}
