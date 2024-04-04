using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }

        public StudentUpdated(Guid studentId)
        {
            StudentId = studentId;
        }
    }    
}
