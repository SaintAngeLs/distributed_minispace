using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentTwoFactorDisabled : IDomainEvent
    {
        public User User { get; }

        public StudentTwoFactorDisabled(User student)
        {
            User = student;
        }
    }
}