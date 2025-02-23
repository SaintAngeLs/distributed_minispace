using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentLanguagesUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentLanguagesUpdated(User student)
        {
            User = student;
        }
    }
}
