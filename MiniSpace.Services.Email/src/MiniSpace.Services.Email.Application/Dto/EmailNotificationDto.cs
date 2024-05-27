using System;

namespace MiniSpace.Services.Email.Application.Dto
{
    public class EmailNotificationDto
    {
        public Guid EmailNotificationId { get; set; }
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
