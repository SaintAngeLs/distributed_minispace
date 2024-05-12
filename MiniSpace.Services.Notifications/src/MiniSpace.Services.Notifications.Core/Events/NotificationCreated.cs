using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Core.Events
{
    public class NotificationCreated : IDomainEvent
    {
        public Guid NotificationId { get; }
        public string UserId { get; }
        public string Message { get; }

        public NotificationCreated(Guid notificationId, string userId, string message)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
        }
    }
}