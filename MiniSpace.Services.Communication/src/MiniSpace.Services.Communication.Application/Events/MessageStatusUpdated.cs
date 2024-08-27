using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class MessageStatusUpdated : IEvent
    {
        public Guid ChatId { get; }
        public Guid MessageId { get; }
        public string Status { get; }

        public MessageStatusUpdated(Guid chatId, Guid messageId, string status)
        {
            ChatId = chatId;
            MessageId = messageId;
            Status = status;
        }
    }
}
