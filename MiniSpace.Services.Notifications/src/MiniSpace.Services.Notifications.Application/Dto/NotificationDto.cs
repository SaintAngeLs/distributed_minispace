using System;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; } 
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }  
        public Guid? RelatedEntityId { get; set; }
        public NotificationEventType EventType { get; set; } 
    }
}
