using Convey.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents
{
    public class StudentNotificationsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public List<NotificationDocument> Notifications { get; set; }
    }
}
