using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserWorkUpdated : IDomainEvent
    {
        public User User { get; }

        public UserWorkUpdated(User student)
        {
            User = student;
        }
    }
}
