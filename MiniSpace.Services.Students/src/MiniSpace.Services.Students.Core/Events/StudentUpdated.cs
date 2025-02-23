using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentUpdated(User student)
        {
            User = student;
        }
    }    
}
