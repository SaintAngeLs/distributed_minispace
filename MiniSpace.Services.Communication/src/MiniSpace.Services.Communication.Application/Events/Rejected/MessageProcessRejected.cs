using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events.Rejected
{
    public class MessageProcessRejected : IRejectedEvent
    {
        public Guid MessageId { get; }
        public string Reason { get; }
        public string Code { get; }

        public MessageProcessRejected(Guid messageId, string reason, string code)
        {
            MessageId = messageId;
            Reason = reason;
            Code = code;
        }
    }
}
