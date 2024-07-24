using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentWorkUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentWorkUpdated(Student student)
        {
            Student = student;
        }
    }
}
