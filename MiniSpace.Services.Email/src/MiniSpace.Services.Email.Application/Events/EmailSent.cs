using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Email.Application.Events
{
    public class EmailSent : IEvent
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
