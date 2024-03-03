using Convey.CQRS.Events;

namespace Pacco.Services.Operations.Api.Types
{
    public class RejectedEvent : IRejectedEvent, IMessage
    {
        public string Reason { get; set; }
        public string Code { get; set; }
    }
}