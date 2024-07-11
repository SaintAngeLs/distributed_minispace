using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentNotificationPreferencesUpdated : IDomainEvent
    {
        public Guid StudentId { get; }
        public NotificationPreferences NotificationPreferences { get; }

        public StudentNotificationPreferencesUpdated(UserNotifications userNotifications)
        {
            StudentId = userNotifications.StudentId;
            NotificationPreferences = userNotifications.NotificationPreferences;
        }
    }
}
