using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentProfileImageRemoved : IDomainEvent
    {
        public User User { get; }

        public StudentProfileImageRemoved(User student)
        {
            User = student;
        }
    }
}
