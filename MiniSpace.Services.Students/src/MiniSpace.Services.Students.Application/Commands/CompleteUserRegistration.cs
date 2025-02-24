using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class CompleteUserRegistration : ICommand
    {
        public Guid UserId { get; }
        public string ProfileImage { get; }
        public string Description { get; }
        public DateTime DateOfBirth { get; }
        public bool EmailNotifications { get; }

        public CompleteUserRegistration(Guid userId, string profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications)
        {
            UserId = userId;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
        }
    }    
}
