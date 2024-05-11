using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class ShowInterestInEventRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public ShowInterestInEventRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}