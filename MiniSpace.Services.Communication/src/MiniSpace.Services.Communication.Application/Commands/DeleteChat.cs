using Paralax.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class DeleteChat : ICommand
    {
        public Guid ChatId { get; set; }

        public Guid UserId { get; set; }

        public DeleteChat(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
