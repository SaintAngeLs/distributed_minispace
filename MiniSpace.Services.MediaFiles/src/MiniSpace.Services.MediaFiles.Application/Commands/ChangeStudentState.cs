using Convey.CQRS.Commands;

namespace MiniSpace.Services.MediaFiles.Application.Commands
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
