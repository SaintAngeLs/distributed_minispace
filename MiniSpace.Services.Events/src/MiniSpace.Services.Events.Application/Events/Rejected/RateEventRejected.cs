using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class RateEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public RateEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}