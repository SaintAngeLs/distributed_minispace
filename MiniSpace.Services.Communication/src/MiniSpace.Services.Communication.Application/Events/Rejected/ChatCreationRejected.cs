using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events.Rejected
{
    public class ChatCreationRejected : IRejectedEvent
    {
        public Guid ChatId { get; }
        public string Reason { get; }
        public string Code { get; }

        public ChatCreationRejected(Guid chatId, string reason, string code)
        {
            ChatId = chatId;
            Reason = reason;
            Code = code;
        }
    }
}
