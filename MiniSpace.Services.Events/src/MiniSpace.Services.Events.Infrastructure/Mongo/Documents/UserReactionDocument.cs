using System;
using System.Collections.Generic;
using Convey.Types;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class UserReactionDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public IEnumerable<ReactionDocument> Reactions { get; set; } 
    }
}
