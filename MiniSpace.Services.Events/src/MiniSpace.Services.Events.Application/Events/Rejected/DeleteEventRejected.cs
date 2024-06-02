using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class DeleteEventRejected: IRejectedEvent
    {
        public Guid EventId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeleteEventRejected(Guid eventId, string reason, string code)
        {
            EventId = eventId;
            Reason = reason;
            Code = code;
        }
    }
}