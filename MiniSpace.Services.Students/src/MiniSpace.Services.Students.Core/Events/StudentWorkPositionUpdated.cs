using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentWorkPositionUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentWorkPositionUpdated(Student student)
        {
            Student = student;
        }
    }
}
