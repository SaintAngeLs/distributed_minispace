using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("notifications")]
    public class NotificationDeleted : IEvent
    {
        public Guid NotificationId { get; }

        public NotificationDeleted(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
