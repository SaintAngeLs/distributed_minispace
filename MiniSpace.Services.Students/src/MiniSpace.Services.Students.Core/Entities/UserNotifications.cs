using System;
using MiniSpace.Services.Students.Core.Events;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserNotifications : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        public NotificationPreferences NotificationPreferences { get; private set; }

        public UserNotifications(Guid studentId, NotificationPreferences notificationPreferences)
        {
            StudentId = studentId;
            NotificationPreferences = notificationPreferences ?? new NotificationPreferences();
        }

        public void UpdatePreferences(NotificationPreferences notificationPreferences)
        {
            NotificationPreferences = notificationPreferences ?? new NotificationPreferences();
            AddEvent(new StudentNotificationPreferencesUpdated(this));
        }
    }
}