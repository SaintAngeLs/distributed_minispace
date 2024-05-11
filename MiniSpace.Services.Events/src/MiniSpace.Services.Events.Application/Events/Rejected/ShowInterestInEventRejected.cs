using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class ShowInterestInEventRejected: IRejectedEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public ShowInterestInEventRejected(Guid eventId, Guid studentId, string reason, string code)
        {
            EventId = eventId;
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }
}