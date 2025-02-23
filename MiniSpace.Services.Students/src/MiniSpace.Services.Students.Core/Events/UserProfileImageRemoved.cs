using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserProfileImageRemoved : IDomainEvent
    {
        public User User { get; }

        public UserProfileImageRemoved(User student)
        {
            User = student;
        }
    }
}
