using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpacePwa.DTO.Enums;

namespace MiniSpacePwa.DTO.Notifications
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; } 
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }  
        public Guid? RelatedEntityId { get; set; }
        public NotificationEventType EventType { get; set; }
        public string Details { get; set; }
    }
}