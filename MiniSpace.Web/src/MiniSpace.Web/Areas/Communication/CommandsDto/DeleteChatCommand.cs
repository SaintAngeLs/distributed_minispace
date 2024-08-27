using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class DeleteChatCommand
    {
        public Guid ChatId { get; set; }

        public DeleteChatCommand(Guid chatId)
        {
            ChatId = chatId;
        }
    }
}
