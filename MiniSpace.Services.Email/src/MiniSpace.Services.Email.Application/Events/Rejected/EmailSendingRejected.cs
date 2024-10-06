using Paralax.CQRS.Events;

namespace MiniSpace.Services.Email.Application.Events.Rejected
{
    public class EmailSendingRejected : IRejectedEvent
    {
        public Guid EmailNotificationId { get; }
        public string Reason { get; }
        public string Code { get; }

        public EmailSendingRejected(Guid emailNotificationId, string reason, string code)
        {
            EmailNotificationId = emailNotificationId;
            Reason = reason;
            Code = code;
        }
    }
}