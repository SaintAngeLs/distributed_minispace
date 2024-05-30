using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Email.Application.Events
{
    public class EmailCreated : IEvent
    {
        public Guid EmailNotificationId { get; }
        public Guid UserId { get; }

        public EmailCreated(Guid emailNotificationId, Guid userId)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
        }
    }
}
