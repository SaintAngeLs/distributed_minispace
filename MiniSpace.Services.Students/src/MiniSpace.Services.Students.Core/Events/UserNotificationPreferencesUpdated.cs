using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserNotificationPreferencesUpdated : IDomainEvent
    {
        public Guid UserId { get; }
        public NotificationPreferences NotificationPreferences { get; }

        public UserNotificationPreferencesUpdated(UserNotifications userNotifications)
        {
            UserId = userNotifications.UserId;
            NotificationPreferences = userNotifications.NotificationPreferences;
        }
    }
}
