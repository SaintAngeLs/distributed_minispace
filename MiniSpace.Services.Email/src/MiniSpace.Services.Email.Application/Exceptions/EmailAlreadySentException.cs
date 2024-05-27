namespace MiniSpace.Services.Email.Application.Exceptions
{
    public class EmailAlreadySentException : AppException
    {
        public override string Code { get; } = "email_already_sent";
        public Guid EmailNotificationId { get; }

        public EmailAlreadySentException(Guid emailNotificationId)
            : base($"Email with ID: {emailNotificationId} has already been sent and cannot be modified.")
        {
            EmailNotificationId = emailNotificationId;
        }
    }
}
