using System;

namespace MiniSpace.Services.Communication.Application.Exceptions
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
