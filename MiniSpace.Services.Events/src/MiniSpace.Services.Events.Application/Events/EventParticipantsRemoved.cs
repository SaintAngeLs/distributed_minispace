using System;
using System.Collections.Generic;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events
{
    public class EventParticipantsRemoved: IEvent
    {
        public Guid EventId { get; }
        public IEnumerable<Guid> Participants { get; }

        public EventParticipantsRemoved(Guid eventId, IEnumerable<Guid> participants)
        {
            EventId = eventId;
            Participants = participants;
        }
    }
}