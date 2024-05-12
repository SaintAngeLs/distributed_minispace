using Convey.CQRS.Commands;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class CreateNotification : ICommand
    {
        public Guid NotificationId { get; }
        public string UserId { get; }
        public string Message { get; }

        public CreateNotification(Guid notificationId, string userId, string message)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
        }
    }
}
