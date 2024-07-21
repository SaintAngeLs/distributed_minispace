using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentCompanyUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentCompanyUpdated(Student student)
        {
            Student = student;
        }
    }
}
