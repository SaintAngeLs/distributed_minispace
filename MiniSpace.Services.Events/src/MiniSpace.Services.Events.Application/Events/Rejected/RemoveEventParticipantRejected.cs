using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class RemoveEventParticipantRejected: IRejectedEvent
    {
        public Guid EventId { get; }
        public string Reason { get; }
        public string Code { get; }

        public RemoveEventParticipantRejected(Guid eventId, string reason, string code)
        {
            EventId = eventId;
            Reason = reason;
            Code = code;
        }
    }
}