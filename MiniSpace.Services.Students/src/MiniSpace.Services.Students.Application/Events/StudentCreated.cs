using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public string ProfileImageUrl { get; }

        public StudentCreated(Guid studentId, string fullName, string profileImageUrl)
        {
            StudentId = studentId;
            FullName = fullName;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
