using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
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