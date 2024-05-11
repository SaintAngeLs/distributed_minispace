using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class SearchOrganizerEventsRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public SearchOrganizerEventsRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}