using Paralax.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public class UserChatsDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<ChatDocument> Chats { get; set; }
        public UserChatsDocument()
        {
            Chats = new List<ChatDocument>();
        }
    }
}
