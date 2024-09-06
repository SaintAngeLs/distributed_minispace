using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Application.Commands
{
    public class CreateNotification : ICommand
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Message { get; }
        public IEnumerable<Guid> UsersId { get; }
        public Guid? EventId { get; }
        public Guid? PostId { get; }
        public Guid? OrganizationId { get; }
        public string NotificationType { get; }

        public CreateNotification(Guid notificationId, Guid userId, string message, 
        IEnumerable<Guid> userIds, string notificationType, 
        Guid? eventId = null, Guid? postId = null, Guid? organizationId = null)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            UsersId = userIds;
            NotificationType = notificationType;
            EventId = eventId;
            PostId = postId;
            OrganizationId = organizationId;
        }
    }
}
