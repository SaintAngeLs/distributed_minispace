using System;
using System.Collections.Generic;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class CreateChatCommand
    {
        public Guid ChatId { get; set; }
        public List<Guid> ParticipantIds { get; set; }
        public string ChatName { get; set; }

        public CreateChatCommand(Guid chatId, List<Guid> participantIds, string chatName = null)
        {
            ChatId = chatId;
            ParticipantIds = participantIds ?? new List<Guid>();
            ChatName = chatName;
        }
    }
}
