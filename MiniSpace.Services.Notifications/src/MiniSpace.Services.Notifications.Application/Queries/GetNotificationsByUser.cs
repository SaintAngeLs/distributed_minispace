using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetNotificationsByUser : IQuery<IEnumerable<NotificationDto>>
    {
        public Guid UserId { get; set; }

        public GetNotificationsByUser(Guid userId)
        {
            UserId = userId;
        }
    }
}
