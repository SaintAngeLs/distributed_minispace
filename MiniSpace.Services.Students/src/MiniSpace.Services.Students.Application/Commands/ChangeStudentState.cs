using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class ChangeStudentState : ICommand
    {
        public Guid StudentId { get; }
        public string State { get; }

        public ChangeStudentState(Guid studentId, string state)
        {
            StudentId = studentId;
            State = state;
        }
    }    
}
