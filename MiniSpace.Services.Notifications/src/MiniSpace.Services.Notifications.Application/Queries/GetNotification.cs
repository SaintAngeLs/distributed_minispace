using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetNotification : IQuery<NotificationDto>
    {
        public Guid NotificationId { get; set; }

        public GetNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
