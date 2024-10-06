using Paralax.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class DeleteMessage : ICommand
    {
        [FromRoute]
        public Guid MessageId { get; }

        [FromQuery]
        public Guid ChatId { get; }

        public DeleteMessage(Guid messageId, Guid chatId)
        {
            MessageId = messageId;
            ChatId = chatId;
        }
    }
}
