using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class AddUserToChatCommand
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }

        public AddUserToChatCommand(Guid chatId, Guid userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
