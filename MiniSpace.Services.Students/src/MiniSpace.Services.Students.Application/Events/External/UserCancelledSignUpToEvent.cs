using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class UserCancelledSignUpToEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public UserCancelledSignUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    } 
}