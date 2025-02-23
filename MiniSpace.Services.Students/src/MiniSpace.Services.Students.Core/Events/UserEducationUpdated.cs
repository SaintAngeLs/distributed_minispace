using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserEducationUpdated : IDomainEvent
    {
        public User User { get; }

        public UserEducationUpdated(User student)
        {
            User = student;
        }
    }
}
