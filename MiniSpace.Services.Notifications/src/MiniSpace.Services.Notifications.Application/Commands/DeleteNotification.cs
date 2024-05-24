using Convey.CQRS.Commands;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class DeleteNotification : ICommand
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; }

        public DeleteNotification(Guid userId, Guid notificationId)
        {
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}
