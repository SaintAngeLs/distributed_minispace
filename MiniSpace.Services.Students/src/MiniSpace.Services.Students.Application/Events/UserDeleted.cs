using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class UserDeleted : IEvent
    {
        public Guid UserId { get; }
        public string FullName { get; }

        public UserDeleted(Guid userId, string fullName)
        {
            UserId = userId;
            FullName = fullName;
        }
    }
}
