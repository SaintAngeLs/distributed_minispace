using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudent : ICommand
    {
        public Guid StudentId { get; }
        public Guid ProfileImage { get; }
        public string Description { get; }
        public bool EmailNotifications { get; }
        
        public UpdateStudent(Guid studentId, Guid profileImage, string description, bool emailNotifications)
        {
            StudentId = studentId;
            ProfileImage = profileImage;
            Description = description;
            EmailNotifications = emailNotifications;
        }
    }    
}
