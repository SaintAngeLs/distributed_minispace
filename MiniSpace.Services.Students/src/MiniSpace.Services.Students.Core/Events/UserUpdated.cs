using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserUpdated : IDomainEvent
    {
        public User User { get; }

        public UserUpdated(User student)
        {
            User = student;
        }
    }    
}
