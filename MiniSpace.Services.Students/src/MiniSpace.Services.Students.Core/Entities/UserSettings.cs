using System;
using MiniSpace.Services.Students.Core.Events;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserSettings : AggregateRoot
    {
        public Guid StudentId { get; private set; }
        public UserAvailableSettings AvailableSettings { get; private set; }

        public UserSettings(Guid studentId, UserAvailableSettings availableSettings)
        {
            StudentId = studentId;
            AvailableSettings = availableSettings ?? new UserAvailableSettings();
        }

        public void UpdateSettings(UserAvailableSettings availableSettings)
        {
            AvailableSettings = availableSettings ?? new UserAvailableSettings();
            AddEvent(new UserSettingsUpdated(this));
        }
    }
}
