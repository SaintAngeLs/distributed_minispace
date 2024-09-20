using System;
using System.Collections.Generic;
using Convey.Types;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class UserCommentsDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public IEnumerable<CommentDocument> Comments { get; set; } 
    }
}
