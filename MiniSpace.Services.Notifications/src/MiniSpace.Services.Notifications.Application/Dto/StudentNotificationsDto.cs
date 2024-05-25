using System;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class StudentNotificationsDto
    {
        private List<Notification> notifications = new List<Notification>();
        public Guid StudentId { get; private set; }

        public StudentNotificationsDto(Guid studentId)
        {
            StudentId = studentId;
        }

        public void AddNotification(Notification notification)
        {
            notifications.Add(notification);
        }

        public IEnumerable<Notification> Notifications => notifications.AsReadOnly();
    }
}
