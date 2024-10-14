using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class UpdateEventRejected: IRejectedEvent
    {
        public Guid EventId { get; }
        public string Reason { get; }
        public string Code { get; }

        public UpdateEventRejected(Guid eventId, string reason, string code)
        {
            EventId = eventId;
            Reason = reason;
            Code = code;
        }
    }
}