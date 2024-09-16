using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Events
{
    [Message("events")]
    public class StudentSignedUpToEvent: IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        
        public StudentSignedUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}