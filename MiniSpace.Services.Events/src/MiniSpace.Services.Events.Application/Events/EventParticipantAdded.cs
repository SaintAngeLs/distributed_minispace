using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventParticipantAdded: IEvent
    {
        public Guid EventId { get; }
        public Guid ParticipantId { get; }

        public EventParticipantAdded(Guid eventId, Guid participantId)
        {
            EventId = eventId;
            ParticipantId = participantId;
        }
    }
}