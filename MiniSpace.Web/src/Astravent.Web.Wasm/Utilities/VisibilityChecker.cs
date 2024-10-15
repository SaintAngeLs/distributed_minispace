using System;
using System.Linq;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Friends;

namespace Astravent.Web.Wasm.Utilities
{
    public class VisibilityChecker
    {
        private readonly Guid _currentUserId;
        private readonly IEnumerable<FriendDto> _allFriends;

        public VisibilityChecker(Guid currentUserId, IEnumerable<FriendDto> allFriends)
        {
            _currentUserId = currentUserId;
            _allFriends = allFriends;
        }

        // Method to check if a profile should be visible based on connection visibility
        public bool ShouldShowProfile(StudentDto student)
        {
            var visibility = student.UserSettings.ConnectionVisibility;
            return visibility == Visibility.Everyone || 
                   (visibility == Visibility.Connections && IsConnection(student.Id));
        }

        // Method to check if the name should be visible
        public bool ShouldShowName(StudentDto student)
        {
            var visibility = student.UserSettings.ConnectionVisibility;
            return visibility == Visibility.Everyone || 
                   (visibility == Visibility.Connections && IsConnection(student.Id));
        }

        // Method to check if the work position should be visible
        public bool ShouldShowWorkPosition(StudentDto student)
        {
            var visibility = student.UserSettings.WorkPositionVisibility;
            return visibility == Visibility.Everyone || 
                   (visibility == Visibility.Connections && IsConnection(student.Id));
        }

        // Method to check if education details should be visible
        public bool ShouldShowEducation(StudentDto student)
        {
            var visibility = student.UserSettings.EducationVisibility;
            return visibility == Visibility.Everyone || 
                   (visibility == Visibility.Connections && IsConnection(student.Id));
        }

        private bool IsConnection(Guid studentId)
        {
            return _allFriends.Any(f => f.FriendId == studentId);
        }
    }
}
