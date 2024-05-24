using System;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class Notification : AggregateRoot
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public NotificationEventType? EventType { get; set; } 

        public Notification(Guid notificationId, 
                            Guid userId, 
                            string message, 
                            NotificationStatus status, 
                            DateTime createdAt, 
                            DateTime? updatedAt, 
                            Guid? relatedEntityId = null,
                            NotificationEventType? eventType = null)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            RelatedEntityId = relatedEntityId;
            EventType = eventType; 
        }

        public void MarkAsRead()
        {
            if (Status != NotificationStatus.Deleted)
            {
                Status = NotificationStatus.Read;
                UpdatedAt = DateTime.UtcNow; 
            }
        }

        public void MarkAsDeleted()
        {
            Status = NotificationStatus.Deleted;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
