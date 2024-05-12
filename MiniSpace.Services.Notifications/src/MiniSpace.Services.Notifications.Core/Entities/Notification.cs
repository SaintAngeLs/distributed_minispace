using System;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class Notification : AggregateRoot
    {
        public Guid NotificationId { get;  set; }
        public string UserId { get;  set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 

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
