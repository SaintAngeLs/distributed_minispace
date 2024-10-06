using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Events.Application.Events
{
    public class StudentShowedInterestInEvent: IEvent

    {
        public Guid EventId { get; }
        public Guid StudentId { get; }

        public StudentShowedInterestInEvent(Guid eventId, Guid studentId)
        {
            EventId = eventId;
            StudentId = studentId;
        }
    }
}