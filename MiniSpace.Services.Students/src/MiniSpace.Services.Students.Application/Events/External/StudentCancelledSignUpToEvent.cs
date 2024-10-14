using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class StudentCancelledSignUpToEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentCancelledSignUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    } 
}