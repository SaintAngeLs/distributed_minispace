using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class DeleteStudent : ICommand
    {
        public Guid StudentId;

        public DeleteStudent(Guid studentId) => StudentId = studentId;
    }    
}
