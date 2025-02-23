using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserWorkPositionUpdated : IDomainEvent
    {
        public User User { get; }

        public UserWorkPositionUpdated(User student)
        {
            User = student;
        }
    }
}
