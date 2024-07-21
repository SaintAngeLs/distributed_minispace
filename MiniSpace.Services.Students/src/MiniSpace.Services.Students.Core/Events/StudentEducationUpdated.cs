using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentEducationUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentEducationUpdated(Student student)
        {
            Student = student;
        }
    }
}
