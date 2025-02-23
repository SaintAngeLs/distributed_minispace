using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserStateChanged : IDomainEvent
    {
        public User User { get; }
        public State PreviousState { get; }

        public UserStateChanged(User student, State previousState)
        {
            User = student;
            PreviousState = previousState;
        }
    }    
}
