using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public Guid MediaFileId { get; }

        public StudentUpdated(Guid studentId, string fullName, Guid mediaFileId)
        {
            StudentId = studentId;
            FullName = fullName;
            MediaFileId = mediaFileId;
        }
    }    
}
