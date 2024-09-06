using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events.Rejected
{
    public class MessageSendRejected : IRejectedEvent
    {
        public Guid ChatId { get; }
        public Guid MessageId { get; }
        public string Reason { get; }
        public string Code { get; }

        public MessageSendRejected(Guid chatId, Guid messageId, string reason, string code)
        {
            ChatId = chatId;
            MessageId = messageId;
            Reason = reason;
            Code = code;
        }
    }

}
