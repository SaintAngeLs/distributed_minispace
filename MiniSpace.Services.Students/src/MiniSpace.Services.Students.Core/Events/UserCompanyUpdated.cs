using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserCompanyUpdated : IDomainEvent
    {
        public User User { get; }

        public UserCompanyUpdated(User student)
        {
            User = student;
        }
    }
}
