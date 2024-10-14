using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Posts.Application.Events.External
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
