using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class CancelInterestInEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public CancelInterestInEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}