using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class UpdateEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public UpdateEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}