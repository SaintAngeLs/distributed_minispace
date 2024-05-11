using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class RemoveEventParticipantRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public RemoveEventParticipantRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}