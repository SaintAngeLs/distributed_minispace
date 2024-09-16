using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

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