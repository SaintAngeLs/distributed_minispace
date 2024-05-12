using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentDeleted : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public Guid MediaFileId { get; }

        public StudentDeleted(Guid studentId, string fullName, Guid mediaFileId)
        {
            StudentId = studentId;
            FullName = fullName;
            MediaFileId = mediaFileId;
        }
    }
}
