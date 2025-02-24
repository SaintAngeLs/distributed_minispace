using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class UserCreated : IEvent
    {
        public Guid UserId { get; }
        public string FullName { get; }
        public string ProfileImageUrl { get; }

        public UserCreated(Guid userId, string fullName, string profileImageUrl)
        {
            UserId = userId;
            FullName = fullName;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
