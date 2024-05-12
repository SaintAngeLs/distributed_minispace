using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("events")]
    public class EventDeleted : IEvent
    {
        public Guid EventId { get; }

        public EventDeleted(Guid eventId)
        {
            EventId = eventId;
        }
    }  
}