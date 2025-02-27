using System;
using Astravent.Web.Wasm.DTO;

namespace Astravent.Web.Wasm.Utilities
{
    public class VisibilityChecker
    {
        // Method to check if a profile should be visible based on connection visibility
        public bool ShouldShowProfile(UserDto user)
        {
            return user?.UserSettings?.ConnectionVisibility == Visibility.Everyone;
        }

        // Method to check if the name should be visible
        public bool ShouldShowName(UserDto user)
        {
            return user?.UserSettings?.ConnectionVisibility == Visibility.Everyone;
        }

        // Method to check if the work position should be visible
        public bool ShouldShowWorkPosition(UserDto user)
        {
            return user?.UserSettings?.WorkPositionVisibility == Visibility.Everyone;
        }

        // Method to check if education details should be visible
        public bool ShouldShowEducation(UserDto user)
        {
            return user?.UserSettings?.EducationVisibility == Visibility.Everyone;
        }

        public bool ShouldShowOnlineStatus(UserDto user)
        {
            return user?.UserSettings?.IsOnlineVisibility == Visibility.Everyone;
        }

        public bool ShouldShowDeviceType(UserDto user)
        {
            return user?.UserSettings?.DeviceTypeVisibility == Visibility.Everyone;
        }

        public bool ShouldShowLastActive(UserDto user)
        {
            return user?.UserSettings?.LastActiveVisibility == Visibility.Everyone;
        }
    }
}
