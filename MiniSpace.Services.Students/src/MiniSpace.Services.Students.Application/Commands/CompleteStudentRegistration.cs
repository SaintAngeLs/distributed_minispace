using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class CompleteStudentRegistration : ICommand
    {
        public Guid StudentId { get; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string FullName => $"{Name} {Surname}";
        public string ProfileImage { get; private set; }
        public string Description { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }

        public CompleteStudentRegistration(Guid studentId, string name, string surname, string profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications)
        {
            StudentId = studentId;
            Name = name;
            Surname = surname;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
        }
    }    
}
