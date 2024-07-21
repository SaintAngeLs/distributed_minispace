using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentBannerUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentBannerUpdated(Student student)
        {
            Student = student;
        }
    }
}
