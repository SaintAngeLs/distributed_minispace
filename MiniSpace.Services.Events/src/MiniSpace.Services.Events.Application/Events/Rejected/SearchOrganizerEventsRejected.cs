using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
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