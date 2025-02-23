
using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserTwoFactorEnabled : IDomainEvent
    {
        public User User { get; }

        public UserTwoFactorEnabled(User student)
        {
            User = student;
        }
    }
}