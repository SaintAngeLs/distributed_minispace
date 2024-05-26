using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventParticipantRemoved: IEvent
    {
        public Guid EventId { get; }
        public Guid Participant { get; }

        public EventParticipantRemoved(Guid eventId, Guid participant)
        {
            EventId = eventId;
            Participant = participant;
        }
    }
}