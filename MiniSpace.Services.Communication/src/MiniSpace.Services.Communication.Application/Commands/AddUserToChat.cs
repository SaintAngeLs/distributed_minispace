using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class AddUserToChat : ICommand
    {
        public Guid ChatId { get; }
        public Guid UserId { get; }

        public AddUserToChat(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
