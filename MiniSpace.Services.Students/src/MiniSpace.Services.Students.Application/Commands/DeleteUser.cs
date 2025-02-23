using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class DeleteStudent : ICommand
    {
        public Guid StudentId { get; }

        public DeleteStudent(Guid studentId) => StudentId = studentId;
    }    
}
