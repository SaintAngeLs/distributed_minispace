using Convey.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class AddEventParticipantRejected: IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public AddEventParticipantRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}