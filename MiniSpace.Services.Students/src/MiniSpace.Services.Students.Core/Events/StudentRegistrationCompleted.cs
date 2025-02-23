using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentRegistrationCompleted : IDomainEvent
    {
        public User User { get; }

        public StudentRegistrationCompleted(User student)
        {
            User = student;
        }
    }    
}
