using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Documents
{
    public class StudentNotificationsDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]  
        public Guid Id { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid StudentId { get; set; }
        public List<NotificationDocument> Notifications { get; set; }
    }
}
