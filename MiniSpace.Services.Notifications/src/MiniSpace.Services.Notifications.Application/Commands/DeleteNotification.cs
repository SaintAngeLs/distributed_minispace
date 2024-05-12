using Convey.CQRS.Commands;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class DeleteNotification : ICommand
    {
        public Guid NotificationId { get; }

        public DeleteNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
