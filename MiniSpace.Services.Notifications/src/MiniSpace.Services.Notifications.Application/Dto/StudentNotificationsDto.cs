using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class StudentNotificationsDto
    {
        public Guid StudentId { get; set; }
        public IEnumerable<NotificationDto> Notifications { get; set; }

        public StudentNotificationsDto()
        {
            Notifications = new List<NotificationDto>();
        }
    }
}
