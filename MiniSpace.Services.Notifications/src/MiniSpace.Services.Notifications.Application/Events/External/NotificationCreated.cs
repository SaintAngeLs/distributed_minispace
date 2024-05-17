using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("friends")]
    public class NotificationCreated : IEvent
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
