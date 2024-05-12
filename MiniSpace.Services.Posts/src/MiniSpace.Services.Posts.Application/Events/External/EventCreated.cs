using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Posts.Application.Events.External
{
    [Message("events")]
    public class EventCreated : IEvent
    {
        public Guid EventId { get; }
        public Guid OrganizerId { get; }

        public EventCreated(Guid eventId, Guid organizerId)
        {
            EventId = eventId;
            OrganizerId = organizerId;
        }
    }    
}
