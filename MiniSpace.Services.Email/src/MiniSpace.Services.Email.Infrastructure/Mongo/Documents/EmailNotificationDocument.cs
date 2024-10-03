using Paralax.Types;
using MiniSpace.Services.Email.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Documents
{
    public class EmailNotificationDocument
    {
        public Guid EmailNotificationId { get; set; } 
        public Guid UserId { get; set; }  
        public string EmailAddress { get; set; } 
        public string Subject { get; set; }  
        public string Body { get; set; } 
        public EmailNotificationStatus Status { get; set; }  

        public DateTime CreatedAt { get; set; } 
        public DateTime? SentAt { get; set; } 

        public EmailNotificationDocument()
        {
            CreatedAt = DateTime.UtcNow;  
        }
    }
}
