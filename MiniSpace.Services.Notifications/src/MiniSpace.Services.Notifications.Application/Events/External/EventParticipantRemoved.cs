using System;
using System.Collections.Generic;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events
{
    [Contract]
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