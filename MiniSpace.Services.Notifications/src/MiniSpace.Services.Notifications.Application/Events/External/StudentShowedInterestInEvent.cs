using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events
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