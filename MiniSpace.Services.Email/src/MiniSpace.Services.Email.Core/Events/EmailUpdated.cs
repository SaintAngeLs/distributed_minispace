using System;

namespace MiniSpace.Services.Email.Core.Events
{
    public class EmailUpdated : IDomainEvent
    {
        public Guid EmailNotificationId { get; }
        public Guid UserId { get; }
        public EmailNotificationStatus NewStatus { get; }

        public EmailUpdated(Guid emailNotificationId, Guid userId, EmailNotificationStatus newStatus)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
            NewStatus = newStatus;
        }
    }
}
