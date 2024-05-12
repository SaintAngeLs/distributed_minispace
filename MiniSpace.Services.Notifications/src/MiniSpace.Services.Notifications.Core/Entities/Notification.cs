using System;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class Notification : AggregateRoot
    {
        public Guid NotificationId { get; private set; }
        public string UserId { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public NotificationStatus Status { get; private set; } // Add status property

        public Notification(Guid notificationId, string userId, string message)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            Status = NotificationStatus.Unread; // Default status as Unread on creation
        }

        // Additional methods to manipulate notification status
        public void MarkAsRead()
        {
            if (Status != NotificationStatus.Deleted)
            {
                Status = NotificationStatus.Read;
            }
        }

        public void MarkAsDeleted()
        {
            Status = NotificationStatus.Deleted;
        }
    }
}
