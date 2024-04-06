using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentStateChanged : IEvent
    {
        public Guid StudentId { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public StudentStateChanged(Guid studentId, string currentState, string previousState)
        {
            StudentId = studentId;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }    
}
