using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentRegistrationCompleted : IDomainEvent
    {
        public Student Student { get; }

        public StudentRegistrationCompleted(Student student)
        {
            Student = student;
        }
    }    
}
