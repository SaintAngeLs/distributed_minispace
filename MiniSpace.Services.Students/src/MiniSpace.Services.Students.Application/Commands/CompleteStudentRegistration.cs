using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class CompleteStudentRegistration : ICommand
    {
        public Guid StudentId { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string ProfileImage { get; private set; }
        public string Description { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }

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
