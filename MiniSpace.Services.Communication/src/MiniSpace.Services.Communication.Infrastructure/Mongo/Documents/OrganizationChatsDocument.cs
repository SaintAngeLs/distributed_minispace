using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public class OrganizationChatsDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public List<ChatDocument> Chats { get; set; }
        public OrganizationChatsDocument()
        {
            Chats = new List<ChatDocument>();
        }
    }
}
