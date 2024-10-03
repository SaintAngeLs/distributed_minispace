using Paralax.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class UpdateNotificationStatus : ICommand
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }
        public string Status { get; set; }

        public UpdateNotificationStatus(Guid userId, Guid notificationId, string status)
        {
            UserId = userId;
            NotificationId = notificationId;
            Status = status;
        }
    }
}
