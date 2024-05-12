using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("notifications")]
    public class NotificationCreated : IEvent
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
