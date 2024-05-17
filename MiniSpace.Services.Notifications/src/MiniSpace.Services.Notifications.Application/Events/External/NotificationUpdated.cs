using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("friends")]
    public class NotificationUpdated : IEvent
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
