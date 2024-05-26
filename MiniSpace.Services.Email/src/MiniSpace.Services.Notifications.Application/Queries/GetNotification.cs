using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;
using System;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetNotification : IQuery<NotificationDto>
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }

        public GetNotification(Guid userId, Guid notificationId)
        {
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}
