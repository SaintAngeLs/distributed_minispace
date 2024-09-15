using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("events")]
    public class StudentCancelledSignUpToEvent: IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        
        public StudentCancelledSignUpToEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}