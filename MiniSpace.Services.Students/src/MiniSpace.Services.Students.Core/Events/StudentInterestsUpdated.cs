using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentInterestsUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentInterestsUpdated(Student student)
        {
            Student = student;
        }
    }
}
