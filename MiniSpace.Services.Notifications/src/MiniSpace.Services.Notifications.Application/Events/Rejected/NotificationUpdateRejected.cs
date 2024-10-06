using Paralax.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.Rejected
{
    
    public class NotificationUpdateRejected : IRejectedEvent
    {
        public Guid NotificationId { get; }
        public string Reason { get; }
        public string Code { get; }

        public NotificationUpdateRejected(Guid notificationId, string reason, string code)
        {
            NotificationId = notificationId;
            Reason = reason;
            Code = code;
        }
    }
}
