using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class CompleteStudentRegistration : ICommand
    {
        public Guid StudentId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string ProfileImage { get; }
        public string Description { get; }
        public DateTime DateOfBirth { get; }
        public bool EmailNotifications { get; }

        public CompleteStudentRegistration(Guid studentId, string firstName, string lastname,
            string profileImage, string description, DateTime dateOfBirth, bool emailNotifications)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastname;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
        }
    }    
}
