using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class UpdateMessageStatusCommand
    {
        public Guid ChatId { get; set; }
        public Guid MessageId { get; set; }
        public string Status { get; set; }

        public UpdateMessageStatusCommand(Guid chatId, Guid messageId, string status)
        {
            ChatId = chatId;
            MessageId = messageId;
            Status = status;
        }
    }
}
