using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public class UserNotificationsDocument : IIdentifiable<Guid>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<NotificationDocument> Notifications { get; set; }
    }
}
