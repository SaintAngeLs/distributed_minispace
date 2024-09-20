using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class MessageSent : IEvent
    {
        public Guid ChatId { get; }
        public Guid MessageId { get; }
        public Guid SenderId { get; }
        public string Content { get; }

        public MessageSent(Guid chatId, Guid messageId, Guid senderId, string content)
        {
            ChatId = chatId;
            MessageId = messageId;
            SenderId = senderId;
            Content = content;
        }
    }
}
