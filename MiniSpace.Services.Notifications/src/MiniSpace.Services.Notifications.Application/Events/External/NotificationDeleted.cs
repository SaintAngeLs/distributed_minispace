using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Contract]
    public class NotificationDeleted : IEvent
    {
        public Guid NotificationId { get; }

        public NotificationDeleted(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
