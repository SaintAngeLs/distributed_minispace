using System;

namespace Astravent.Web.Wasm.Areas.Communication.CommandsDto
{
    public class DeleteMessageCommand
    {
        public Guid MessageId { get; set; }
        public Guid ChatId { get; set; }

        public DeleteMessageCommand(Guid messageId, Guid chatId)
        {
            MessageId = messageId;
            ChatId = chatId;
        }
    }
}
