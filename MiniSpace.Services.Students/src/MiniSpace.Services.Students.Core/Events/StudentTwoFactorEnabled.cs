
using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentTwoFactorEnabled : IDomainEvent
    {
        public User User { get; }

        public StudentTwoFactorEnabled(User student)
        {
            User = student;
        }
    }
}