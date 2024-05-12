using System;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class Notification : AggregateRoot
    {
        public Guid NotificationId { get; private set; }
        public string UserId { get; private set; }
        public string Message { get; private set; }
        public NotificationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; } 

        public Notification(Guid notificationId, string userId, string message, NotificationStatus status, DateTime createdAt, DateTime? updatedAt)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
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
