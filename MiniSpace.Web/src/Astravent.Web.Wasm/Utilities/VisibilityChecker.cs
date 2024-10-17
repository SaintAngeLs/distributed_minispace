using System;
using Astravent.Web.Wasm.DTO;

namespace Astravent.Web.Wasm.Utilities
{
    public class VisibilityChecker
    {
        // Method to check if a profile should be visible based on connection visibility
        public bool ShouldShowProfile(StudentDto student)
        {
            return student?.UserSettings?.ConnectionVisibility == Visibility.Everyone;
        }

        // Method to check if the name should be visible
        public bool ShouldShowName(StudentDto student)
        {
            return student?.UserSettings?.ConnectionVisibility == Visibility.Everyone;
        }

        // Method to check if the work position should be visible
        public bool ShouldShowWorkPosition(StudentDto student)
        {
            return student?.UserSettings?.WorkPositionVisibility == Visibility.Everyone;
        }

        // Method to check if education details should be visible
        public bool ShouldShowEducation(StudentDto student)
        {
            return student?.UserSettings?.EducationVisibility == Visibility.Everyone;
        }

        public bool ShouldShowOnlineStatus(StudentDto student)
        {
            return student?.UserSettings?.IsOnlineVisibility == Visibility.Everyone;
        }

        public bool ShouldShowDeviceType(StudentDto student)
        {
            return student?.UserSettings?.DeviceTypeVisibility == Visibility.Everyone;
        }

        public bool ShouldShowLastActive(StudentDto student)
        {
            return student?.UserSettings?.LastActiveVisibility == Visibility.Everyone;
        }
    }
}
