using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class UserStateChanged : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public UserStateChanged(Guid studentId, string fullName, string currentState, string previousState)
        {
            StudentId = studentId;
            FullName = fullName;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }    
}
