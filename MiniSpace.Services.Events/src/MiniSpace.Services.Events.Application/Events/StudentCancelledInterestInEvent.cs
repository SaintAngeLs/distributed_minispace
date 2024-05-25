using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events
{
    public class StudentCancelledInterestInEvent: IEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        
        public StudentCancelledInterestInEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}