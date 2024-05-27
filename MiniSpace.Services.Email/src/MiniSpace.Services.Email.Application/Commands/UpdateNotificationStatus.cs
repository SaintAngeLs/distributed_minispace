using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MiniSpace.Services.Email.Application.Commands
{
    public class UpdateNotificationStatus : ICommand
    {
        [FromRoute]
        public Guid UserId { get; set; }
        
        [FromRoute]
        public Guid NotificationId { get; set; }
        
        [FromQuery]
        public string Status { get; set; }

        public UpdateNotificationStatus(Guid userId, Guid notificationId, string status)
        {
            UserId = userId;
            NotificationId = notificationId;
            Status = status;
        }
    }
}
