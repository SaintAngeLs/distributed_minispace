using System;

namespace MiniSpace.Services.Notifications.Application.Exceptions
{
    public class InvalidNotificationStatusException : AppException
    {
        public override string Code { get; } = "invalid_notification_status";
        
        public InvalidNotificationStatusException(string message)
            : base(message)
        {
        }
    }
}
