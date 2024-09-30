using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Contract]
    public class NotificationDeleted : IEvent
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
