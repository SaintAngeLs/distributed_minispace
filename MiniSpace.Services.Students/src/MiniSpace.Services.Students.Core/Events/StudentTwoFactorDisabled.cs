using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentTwoFactorDisabled : IDomainEvent
    {
        public Student Student { get; }

        public StudentTwoFactorDisabled(Student student)
        {
            Student = student;
        }
    }
}