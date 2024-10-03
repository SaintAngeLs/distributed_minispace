using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events
{
    [Message("events")]
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