using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class CancelInterestInEventRejected: IRejectedEvent
    {
        [ExcludeFromCodeCoverage]
        public Guid EventId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CancelInterestInEventRejected(Guid eventId, string reason, string code)
        {
            EventId = eventId;
            Reason = reason;
            Code = code;
        }
    }
}