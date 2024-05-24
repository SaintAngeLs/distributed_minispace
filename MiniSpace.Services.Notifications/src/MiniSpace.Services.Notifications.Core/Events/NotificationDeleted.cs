using System;

namespace MiniSpace.Services.Notifications.Core.Events
{
    public class NotificationDeleted : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid NotificationId { get; }

        public NotificationDeleted(Guid userId, Guid notificationId)
        {
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}
