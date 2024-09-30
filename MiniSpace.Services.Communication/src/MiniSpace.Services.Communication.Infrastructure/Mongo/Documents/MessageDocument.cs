using Paralax.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MiniSpace.Services.Communication.Core.Entities;
using System;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public class MessageDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType Type { get; set; }
        public MessageStatus Status { get; set; }
    }
}
