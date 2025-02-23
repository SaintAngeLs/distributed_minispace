using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentWorkPositionUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentWorkPositionUpdated(User student)
        {
            User = student;
        }
    }
}
