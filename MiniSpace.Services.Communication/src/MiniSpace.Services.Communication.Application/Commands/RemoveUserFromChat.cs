using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class RemoveUserFromChat : ICommand
    {
        public Guid ChatId { get; }
        public Guid UserId { get; }

        public RemoveUserFromChat(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
