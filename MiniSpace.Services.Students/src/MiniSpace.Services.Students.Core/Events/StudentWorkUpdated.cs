using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentWorkUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentWorkUpdated(User student)
        {
            User = student;
        }
    }
}
