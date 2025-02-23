using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class UserCancelledInterestInEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public UserCancelledInterestInEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    } 
}