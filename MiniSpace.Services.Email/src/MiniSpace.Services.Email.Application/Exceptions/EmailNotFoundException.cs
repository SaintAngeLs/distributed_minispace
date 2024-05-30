namespace MiniSpace.Services.Email.Application.Exceptions
{
    public class EmailNotFoundException : AppException
    {
        public override string Code { get; } = "email_not_found";
        public Guid EmailNotificationId { get; }

        public EmailNotFoundException(Guid emailNotificationId)
            : base($"Email notification with ID: {emailNotificationId} was not found.")
        {
            EmailNotificationId = emailNotificationId;
        }
    }
}
