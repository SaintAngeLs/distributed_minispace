using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserLanguagesUpdated : IDomainEvent
    {
        public User User { get; }

        public UserLanguagesUpdated(User student)
        {
            User = student;
        }
    }
}
