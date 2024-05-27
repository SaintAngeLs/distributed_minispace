using System;

namespace MiniSpace.Services.Email.Core.Events
{
    public class EmailDeleted : IDomainEvent
    {
        public Guid EmailNotificationId { get; }
        public Guid UserId { get; }

        public EmailDeleted(Guid emailNotificationId, Guid userId)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
        }
    }
}
