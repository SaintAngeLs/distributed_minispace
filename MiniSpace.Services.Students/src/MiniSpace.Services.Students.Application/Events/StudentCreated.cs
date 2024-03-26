using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentCreated : IEvent
    {
        public Guid StudentId { get; }

        public StudentCreated(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
