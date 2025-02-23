using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserBannerImageRemoved : IDomainEvent
    {
        public User User { get; }

        public UserBannerImageRemoved(User user)
        {
            User = user;
        }
    }
}
