using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentNotificationPreferencesUpdated : IDomainEvent
    {
        public Guid UserId { get; }
        public NotificationPreferences NotificationPreferences { get; }

        public StudentNotificationPreferencesUpdated(UserNotifications userNotifications)
        {
            UserId = userNotifications.UserId;
            NotificationPreferences = userNotifications.NotificationPreferences;
        }
    }
}
