using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class CancelSignUpToEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public CancelSignUpToEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}