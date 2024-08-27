using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events
{
    public class UserAddedToChat : IEvent
    {
        public Guid ChatId { get; }
        public Guid UserId { get; }

        public UserAddedToChat(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
