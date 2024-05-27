namespace MiniSpace.Services.Email.Application.Exceptions
{
    public class NotificationAlreadyDeletedException : AppException
    {
        public override string Code { get; } = "notification_already_deleted";
        public Guid NotificationId { get; }

         public NotificationAlreadyDeletedException(Guid notificationId)
            : base($"Notification with ID: {notificationId} has already been deleted.")
        {
            NotificationId = notificationId;
        }
    }
}
