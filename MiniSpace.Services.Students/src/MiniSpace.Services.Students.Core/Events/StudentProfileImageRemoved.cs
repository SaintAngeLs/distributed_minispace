using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentProfileImageRemoved : IDomainEvent
    {
        public Student Student { get; }

        public StudentProfileImageRemoved(Student student)
        {
            Student = student;
        }
    }
}
