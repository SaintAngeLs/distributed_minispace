namespace MiniSpace.Services.Emails.Core.Entities
{
    public class EmailNotification : AggregateRoot
    {
        public Guid EmailNotificationId { get; set; }
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailNotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }

        public EmailNotification(Guid emailNotificationId, Guid userId, string emailAddress, string subject, string body, EmailNotificationStatus status)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
            EmailAddress = emailAddress;
            Subject = subject;
            Body = body;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsSent()
        {
            Status = EmailNotificationStatus.Sent;
            SentAt = DateTime.UtcNow;
        }
    }
}
