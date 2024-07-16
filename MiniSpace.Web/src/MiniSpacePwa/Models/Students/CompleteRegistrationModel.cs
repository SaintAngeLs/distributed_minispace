using System;

namespace MiniSpacePwa.Models.Students
{
    public class CompleteRegistrationModel
    {
        public Guid StudentId { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool EmailNotifications { get; set; }
    }
}
