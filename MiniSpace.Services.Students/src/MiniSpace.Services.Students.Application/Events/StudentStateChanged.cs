using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
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
