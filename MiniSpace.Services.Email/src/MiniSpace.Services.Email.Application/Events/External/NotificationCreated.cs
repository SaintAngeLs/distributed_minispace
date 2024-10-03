using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Email.Application.Events.External
{
    [Message("notifications")]
    public class NotificationCreated : IEvent
    {
        public Guid NotificationId { get; private set; }
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string EventType { get; private set; }
        public Guid? RelatedEntityId { get; private set; }
        public string Details { get; private set; }

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
