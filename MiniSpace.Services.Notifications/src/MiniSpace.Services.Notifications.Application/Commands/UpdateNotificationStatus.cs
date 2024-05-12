using Convey.CQRS.Commands;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class UpdateNotificationStatus : ICommand
    {
        public Guid NotificationId { get; }
        public string Status { get; }

        public UpdateNotificationStatus(Guid notificationId, string status)
        {
            NotificationId = notificationId;
            Status = status;
        }
    }
}
