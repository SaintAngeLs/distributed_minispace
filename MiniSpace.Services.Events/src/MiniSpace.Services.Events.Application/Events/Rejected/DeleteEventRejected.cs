using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class DeleteEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public DeleteEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}