using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class UserDeleted : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }

        public UserDeleted(Guid studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
        }
    }
}
