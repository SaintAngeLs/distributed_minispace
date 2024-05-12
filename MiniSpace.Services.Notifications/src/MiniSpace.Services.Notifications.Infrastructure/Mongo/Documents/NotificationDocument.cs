using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public class NotificationDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; } 
        [BsonElement("notificationId")]
        public Guid NotificationId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

    }
}
