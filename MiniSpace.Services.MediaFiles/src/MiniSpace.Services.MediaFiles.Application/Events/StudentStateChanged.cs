using Convey.CQRS.Events;

namespace MiniSpace.Services.MediaFiles.Application.Events
{
    public class StudentStateChanged : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public StudentStateChanged(Guid studentId, string fullName, string currentState, string previousState)
        {
            StudentId = studentId;
            FullName = fullName;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }    
}
