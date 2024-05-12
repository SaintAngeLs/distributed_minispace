using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public class NotificationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid NotificationId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
