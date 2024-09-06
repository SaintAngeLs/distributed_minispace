using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class UserNotifications
    {
        public Guid UserId { get; private set; }
        private List<Notification> _notifications;

        public IEnumerable<Notification> Notifications => _notifications.AsReadOnly();

        public UserNotifications(Guid userId)
        {
            UserId = userId;
            _notifications = new List<Notification>();
        }

        public void AddNotification(Notification notification)
        {
            if (notification != null)
            {
                _notifications.Add(notification);
            }
        }

        public void RemoveNotification(Guid notificationId)
        {
            _notifications.RemoveAll(n => n.NotificationId == notificationId);
        }

        public void MarkNotificationAsRead(Guid notificationId)
        {
            var notification = _notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.MarkAsRead();
            }
        }
    }
}
