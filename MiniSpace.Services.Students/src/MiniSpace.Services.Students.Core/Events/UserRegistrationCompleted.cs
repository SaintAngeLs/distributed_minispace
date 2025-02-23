using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserRegistrationCompleted : IDomainEvent
    {
        public User User { get; }

        public UserRegistrationCompleted(User student)
        {
            User = student;
        }
    }    
}
