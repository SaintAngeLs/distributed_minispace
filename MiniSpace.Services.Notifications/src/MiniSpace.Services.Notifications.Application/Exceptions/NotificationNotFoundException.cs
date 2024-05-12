namespace MiniSpace.Services.Notifications.Application.Exceptions
{
    public class NotificationNotFoundException : AppException
    {
        public override string Code { get; } = "notification_not_found";
        public Guid NotificationId { get; }

        public NotificationNotFoundException(Guid notificationId)
            : base($"Notification with ID: {notificationId} was not found.")
        {
            NotificationId = notificationId;
        }
    }
}
