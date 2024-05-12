using System;

namespace MiniSpace.Services.Notifications.Core.Events
{
    public class NotificationDeleted : IDomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationDeleted(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
