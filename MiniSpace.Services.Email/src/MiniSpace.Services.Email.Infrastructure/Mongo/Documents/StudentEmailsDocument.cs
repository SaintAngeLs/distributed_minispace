using Paralax.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Documents
{
    public class StudentEmailsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; } 

        public List<EmailNotificationDocument> EmailNotifications { get; set; } 

        public StudentEmailsDocument()
        {
            EmailNotifications = new List<EmailNotificationDocument>();
        }
    }
}
