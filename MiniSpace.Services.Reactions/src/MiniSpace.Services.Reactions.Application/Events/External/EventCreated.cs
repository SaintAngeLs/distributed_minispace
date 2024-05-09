using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Reactions.Application.Events.External
{
    [Message("events")]
    public class EventCreated : IEvent
    {
        public Guid EventId { get; }
        public EventCreated(Guid eventId, Guid organizerId)
        {
            EventId = eventId;
        }
    }    
}
