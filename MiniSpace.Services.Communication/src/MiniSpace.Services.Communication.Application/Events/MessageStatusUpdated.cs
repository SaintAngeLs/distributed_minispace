using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class MessageStatusUpdated : IEvent
    {
        public Guid ChatId { get; set; }
        public Guid MessageId { get; set; }
        public string Status { get; set; }

        public MessageStatusUpdated(Guid chatId, Guid messageId, string status)
        {
            ChatId = chatId;
            MessageId = messageId;
            Status = status;
        }
    }
}
