using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class EventSignedUp : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public EventSignedUp(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }    
}
