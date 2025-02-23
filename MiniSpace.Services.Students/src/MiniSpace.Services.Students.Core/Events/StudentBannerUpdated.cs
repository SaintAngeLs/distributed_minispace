using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentBannerUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentBannerUpdated(User student)
        {
            User = student;
        }
    }
}
