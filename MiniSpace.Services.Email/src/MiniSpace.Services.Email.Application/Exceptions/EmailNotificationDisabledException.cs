namespace MiniSpace.Services.Email.Application.Exceptions
{
    public class EmailNotificationDisabledException : AppException
    {
        public override string Code { get; } = "email_notification_disabled";
        public Guid UserId { get; }

        public EmailNotificationDisabledException(Guid userId)
            : base($"Email notifications are disabled for user ID: {userId}.")
        {
            UserId = userId;
        }
    }
}
