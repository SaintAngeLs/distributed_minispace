using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class DeleteChatCommand
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }

        public DeleteChatCommand(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId; 
        }
    }
}
