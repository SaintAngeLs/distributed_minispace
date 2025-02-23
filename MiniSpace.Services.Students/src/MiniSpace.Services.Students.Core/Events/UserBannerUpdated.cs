using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserBannerUpdated : IDomainEvent
    {
        public User User { get; }

        public UserBannerUpdated(User student)
        {
            User = student;
        }
    }
}
