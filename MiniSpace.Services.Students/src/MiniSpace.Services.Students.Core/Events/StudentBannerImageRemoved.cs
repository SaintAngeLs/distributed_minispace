using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentBannerImageRemoved : IDomainEvent
    {
        public Student Student { get; }

        public StudentBannerImageRemoved(Student student)
        {
            Student = student;
        }
    }
}
