using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events
{
    public class EventParticipantAdded: IEvent
    {
        public Guid EventId { get; }
        public Guid ParticipantId { get; }
        public string ParticipantName { get; }

        public EventParticipantAdded(Guid eventId, Guid participantId, string participantName)
        {
            EventId = eventId;
            ParticipantId = participantId;
            ParticipantName = participantName;
        }
    }
}