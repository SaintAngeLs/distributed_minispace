using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class CompleteStudentRegistration : ICommand
    {
        public Guid StudentId { get; }
        public string ProfileImage { get; }
        public string Description { get; }
        public DateTime DateOfBirth { get; }
        public bool EmailNotifications { get; }

        public CompleteStudentRegistration(Guid studentId, string profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications)
        {
            StudentId = studentId;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
        }
    }    
}
