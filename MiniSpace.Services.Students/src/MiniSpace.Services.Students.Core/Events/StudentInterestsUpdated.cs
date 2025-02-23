using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentInterestsUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentInterestsUpdated(User student)
        {
            User = student;
        }
    }
}
