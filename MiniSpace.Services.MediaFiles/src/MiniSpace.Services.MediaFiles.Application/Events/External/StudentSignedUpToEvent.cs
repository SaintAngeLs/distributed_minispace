using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
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
