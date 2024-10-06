using Paralax.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public class ChatDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public List<Guid> ParticipantIds { get; set; }

        public List<MessageDocument> Messages { get; set; }

        public ChatDocument()
        {
            ParticipantIds = new List<Guid>();
            Messages = new List<MessageDocument>();
        }
    }
}
