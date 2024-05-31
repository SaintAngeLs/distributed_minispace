using Convey.CQRS.Commands;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class CreateNotification : ICommand
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Message { get; }
        public IEnumerable<Guid> StudentIds { get; }
        public Guid? EventId { get; }
        public CreateNotification(Guid notificationId, Guid userId, string message, IEnumerable<Guid> studentIds, Guid? eventId = null)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            StudentIds = studentIds;
            EventId = eventId;
        }
    }
}
