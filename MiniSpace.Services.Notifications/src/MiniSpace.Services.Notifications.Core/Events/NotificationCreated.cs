using System;

namespace MiniSpace.Services.Notifications.Core.Events
{
    public class NotificationCreated : IDomainEvent
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EventType { get; set; }  
        public Guid RelatedEntityId { get; set; }  
        public string Details { get; set; } 

        public NotificationCreated(Guid notificationId, Guid userId, string message, DateTime createdAt, string eventType, Guid relatedEntityId, string details)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            CreatedAt = createdAt;
            EventType = eventType;
            RelatedEntityId = relatedEntityId;
            Details = details;
        }
    }
}
