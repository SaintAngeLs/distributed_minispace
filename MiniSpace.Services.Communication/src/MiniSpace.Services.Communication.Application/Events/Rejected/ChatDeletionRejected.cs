using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events.Rejected
{
    public class ChatDeletionRejected : IRejectedEvent
    {
        public Guid ChatId { get; }
        public string Reason { get; }
        public string Code { get; }

        public ChatDeletionRejected(Guid chatId, string reason, string code)
        {
            ChatId = chatId;
            Reason = reason;
            Code = code;
        }
    }
}
