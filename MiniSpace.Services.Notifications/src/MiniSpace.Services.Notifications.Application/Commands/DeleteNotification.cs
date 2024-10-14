using Paralax.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class DeleteNotification : ICommand
    {
        [FromQuery]
        public Guid UserId { get; }

        [FromRoute]
        public Guid NotificationId { get; }

        public DeleteNotification(Guid userId, Guid notificationId)
        {
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}
