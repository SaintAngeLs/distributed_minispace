namespace MiniSpace.Services.Email.Core.Exceptions
{
    public class EmailDeliveryFailedException : DomainException
    {
        public override string Code { get; } = "email_delivery_failed";
        public Guid EmailNotificationId { get; }

        public EmailDeliveryFailedException(Guid emailNotificationId, string message) 
            : base(message)
        {
            EmailNotificationId = emailNotificationId;
        }
    }
}
