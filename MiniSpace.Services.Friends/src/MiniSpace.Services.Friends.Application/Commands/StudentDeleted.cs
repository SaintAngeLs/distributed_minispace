using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class StudentDeleted : ICommand
    {
        public Guid StudentId { get; }

        public StudentDeleted(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
