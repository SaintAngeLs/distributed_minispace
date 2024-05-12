using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class SearchEventsRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public SearchEventsRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}