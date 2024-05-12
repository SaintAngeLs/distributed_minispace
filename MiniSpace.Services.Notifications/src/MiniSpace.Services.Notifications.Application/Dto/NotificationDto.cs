using System;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } 
    }
}
