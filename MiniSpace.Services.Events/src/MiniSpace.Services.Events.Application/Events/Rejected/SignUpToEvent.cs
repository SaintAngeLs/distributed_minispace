using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class SignUpToEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public SignUpToEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}