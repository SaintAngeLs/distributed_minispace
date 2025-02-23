using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class ChangeUserState : ICommand
    {
        public Guid StudentId { get; }
        public string State { get; }

        public ChangeUserState(Guid studentId, string state)
        {
            StudentId = studentId;
            State = state;
        }
    }    
}
