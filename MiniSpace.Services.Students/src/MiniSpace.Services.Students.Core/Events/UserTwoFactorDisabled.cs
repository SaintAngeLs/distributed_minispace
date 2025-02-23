using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserTwoFactorDisabled : IDomainEvent
    {
        public User User { get; }

        public UserTwoFactorDisabled(User student)
        {
            User = student;
        }
    }
}