using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentLanguagesUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentLanguagesUpdated(Student student)
        {
            Student = student;
        }
    }
}
