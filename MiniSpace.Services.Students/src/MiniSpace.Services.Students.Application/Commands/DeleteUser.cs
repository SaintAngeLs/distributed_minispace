using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class DeleteUser : ICommand
    {
        public Guid StudentId { get; }

        public DeleteUser(Guid studentId) => StudentId = studentId;
    }    
}
