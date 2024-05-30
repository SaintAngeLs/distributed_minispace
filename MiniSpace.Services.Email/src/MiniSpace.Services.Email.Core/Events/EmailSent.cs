using System;

namespace MiniSpace.Services.Email.Core.Events
{
    public class EmailSent : IDomainEvent
    {
        public Guid EmailNotificationId { get; }
        public Guid UserId { get; }
        public DateTime SentAt { get; }

        public EmailSent(Guid emailNotificationId, Guid userId, DateTime sentAt)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
            SentAt = sentAt;
        }
    }
}
