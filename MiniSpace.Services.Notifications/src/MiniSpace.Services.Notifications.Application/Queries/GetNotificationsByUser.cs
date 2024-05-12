using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetNotificationsByUser : IQuery<IEnumerable<NotificationDto>>
    {
        public string UserId { get; set; }

        public GetNotificationsByUser(string userId)
        {
            UserId = userId;
        }
    }
}
