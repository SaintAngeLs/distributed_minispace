using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Events
{
     public class NotificationUpdated : IDomainEvent
    {
        public Guid NotificationId { get; }
        public string UserId { get; }
        public NotificationStatus NewStatus { get; }

        public NotificationUpdated(Guid notificationId, string userId, NotificationStatus newStatus)
        {
            NotificationId = notificationId;
            UserId = userId;
            NewStatus = newStatus;
        }
    }
}