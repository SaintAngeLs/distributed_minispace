using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class EventInterestedIn : IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public EventInterestedIn(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }    
}
