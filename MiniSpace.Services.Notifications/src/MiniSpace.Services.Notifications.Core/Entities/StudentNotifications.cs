using System;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class StudentNotifications : IEnumerable<Notification>
    {
        public Guid StudentId { get; private set; }
        private List<Notification> Notifications { get; set; }

        public StudentNotifications(Guid studentId)
        {
            StudentId = studentId;
            Notifications = new List<Notification>();
        }

        public void AddNotification(Notification notification)
        {
            if (notification != null)
            {
                Notifications.Add(notification);
            }
        }

        public IEnumerator<Notification> GetEnumerator()
        {
            return Notifications.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void RemoveNotification(Guid notificationId)
        {
            Notifications.RemoveAll(n => n.NotificationId == notificationId);
        }

        public void MarkNotificationAsRead(Guid notificationId)
        {
            var notification = Notifications.Find(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.MarkAsRead();
            }
        }
    }
}
