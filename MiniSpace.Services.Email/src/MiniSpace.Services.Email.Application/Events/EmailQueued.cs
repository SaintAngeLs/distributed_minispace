using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Email.Application.Events
{
    public class EmailQueued : IEvent
    {
        public Guid EmailNotificationId { get; }
        public Guid UserId { get; }

        public EmailQueued(Guid emailNotificationId, Guid userId)
        {
            EmailNotificationId = emailNotificationId;
            UserId = userId;
        }
    }
}