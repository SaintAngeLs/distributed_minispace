using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentBannerImageRemoved : IDomainEvent
    {
        public User User { get; }

        public StudentBannerImageRemoved(User user)
        {
            User = user;
        }
    }
}
