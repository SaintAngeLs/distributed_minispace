using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO.Notifications
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; } 
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }  

        public bool IsActive { get; set; }
    }
}