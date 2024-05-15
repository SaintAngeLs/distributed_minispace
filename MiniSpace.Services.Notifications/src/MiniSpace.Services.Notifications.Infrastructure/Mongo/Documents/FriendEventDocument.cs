using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public class FriendEventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid EventId { get; set; }  
        public Guid UserId { get; set; } 
        public string EventType { get; set; }  
        public string Details { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
