using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Reports.Application.Events.External
{
    [Message("events")]
    public class EventCreated : IEvent
    {
        public Guid EventId { get; }

        public EventCreated(Guid eventId)
        {
            EventId = eventId;
        }
    }    
}