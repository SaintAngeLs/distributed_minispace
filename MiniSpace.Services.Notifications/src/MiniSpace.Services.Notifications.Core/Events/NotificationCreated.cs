using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Core.Events
{
    public class NotificationCreated : IDomainEvent
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }
        public NotificationCreated(Guid notificationId, Guid userId, string message, DateTime createdAt)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            CreatedAt = createdAt;
        }
    }
}