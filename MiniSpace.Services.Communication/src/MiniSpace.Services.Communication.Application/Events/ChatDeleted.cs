using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class ChatDeleted : IEvent
    {
        public Guid ChatId { get; }

        public ChatDeleted(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
