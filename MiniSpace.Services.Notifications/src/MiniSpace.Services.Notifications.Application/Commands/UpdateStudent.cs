using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid StudentId { get; }
        public string ProfileImage { get; }
        public string Description { get; }
        public bool EmailNotifications { get; }
        
        public UpdateStudent(Guid studentId, string profileImage, string description, bool emailNotifications)
        {
            StudentId = studentId;
            ProfileImage = profileImage;
            Description = description;
            EmailNotifications = emailNotifications;
        }
    }    
}
