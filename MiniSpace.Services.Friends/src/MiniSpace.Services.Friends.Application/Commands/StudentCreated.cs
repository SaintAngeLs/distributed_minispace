using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class StudentCreated : ICommand
    {
        public Guid StudentId { get; }
        public string Email { get; }

        public StudentCreated(Guid studentId, string email)
        {
            StudentId = studentId;
            Email = email;
        }
    }
}
