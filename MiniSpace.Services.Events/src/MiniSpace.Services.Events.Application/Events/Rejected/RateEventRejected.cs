using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class RateEventRejected: IRejectedEvent
    {
        public Guid EventId { get; }
        public Guid StudentId { get; }
        public string Reason { get; }
        public string Code { get; }

        public RateEventRejected(Guid eventId, Guid studentId, string reason, string code)
        {
            EventId = eventId;
            StudentId = studentId;
            Reason = reason;
            Code = code;
        }
    }
}