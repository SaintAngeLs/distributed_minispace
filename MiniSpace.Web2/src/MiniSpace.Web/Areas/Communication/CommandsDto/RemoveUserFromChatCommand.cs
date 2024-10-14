using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class RemoveUserFromChatCommand
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }

        public RemoveUserFromChatCommand(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
