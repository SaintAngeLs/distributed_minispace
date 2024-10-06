using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Email.Application.Events.Rejected
{
    public class EmailCreationRejected : IRejectedEvent
    {
        public Guid EmailNotificationId { get; }
        public string Reason { get; }
        public string Code { get; }

        public EmailCreationRejected(Guid emailNotificationId, string reason, string code)
        {
            EmailNotificationId = emailNotificationId;
            Reason = reason;
            Code = code;
        }
    }
}
