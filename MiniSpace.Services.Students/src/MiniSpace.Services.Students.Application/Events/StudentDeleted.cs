using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentDeleted : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }

        public StudentDeleted(Guid studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
        }
    }
}
