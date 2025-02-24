using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class UserStateChanged : IEvent
    {
        public Guid UserId { get; }
        public string FullName { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public UserStateChanged(Guid userId, string fullName, string currentState, string previousState)
        {
            UserId = userId;
            FullName = fullName;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }    
}
