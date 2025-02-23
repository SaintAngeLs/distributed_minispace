using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserInterestsUpdated : IDomainEvent
    {
        public User User { get; }

        public UserInterestsUpdated(User student)
        {
            User = student;
        }
    }
}
