using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.Rejected
{
    
    public class NotificationDeletionRejected : IRejectedEvent
    {
        public Guid NotificationId { get; }
        public string Reason { get; }
        public string Code { get; }

        public NotificationDeletionRejected(Guid notificationId, string reason, string code)
        {
            NotificationId = notificationId;
            Reason = reason;
            Code = code;
        }
    }
}
