using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

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