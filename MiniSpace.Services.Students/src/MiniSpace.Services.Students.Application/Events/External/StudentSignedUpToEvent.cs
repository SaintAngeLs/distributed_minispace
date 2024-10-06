using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class StudentSignedUpToEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentSignedUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }    
}
