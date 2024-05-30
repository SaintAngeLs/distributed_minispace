namespace MiniSpace.Services.Email.Core.Exceptions
{
    public class EmailAlreadySentException : DomainException
    {
        public override string Code { get; } = "email_already_sent";
        public Guid EmailNotificationId { get; }

        public EmailAlreadySentException(Guid emailNotificationId) 
            : base($"Email with ID '{emailNotificationId}' has already been sent.")
        {
            EmailNotificationId = emailNotificationId;
        }
    }
}
