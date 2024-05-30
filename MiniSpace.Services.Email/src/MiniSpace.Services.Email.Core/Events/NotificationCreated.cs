using System;

namespace MiniSpace.Services.Email.Core.Events
{
    public class NotificationCreated : IDomainEvent
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }
        public string EventType { get; }  
        public Guid? RelatedEntityId { get; }  
        public string Details { get; } 

        public NotificationCreated(Guid notificationId, Guid userId, string message, DateTime createdAt, string eventType, Guid? relatedEntityId, string details)
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
