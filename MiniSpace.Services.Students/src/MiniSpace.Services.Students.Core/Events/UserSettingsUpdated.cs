using System;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserSettingsUpdated : IDomainEvent
    {
        public UserSettings UserSettings { get; }

        public UserSettingsUpdated(UserSettings userSettings)
        {
            UserSettings = userSettings;
        }
    }
}
