using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class DeleteChat : ICommand
    {
        [FromRoute]
        public Guid ChatId { get; }

        public DeleteChat(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
