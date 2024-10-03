using System;

namespace MiniSpace.Web.Areas.Communication.CommandsDto
{
    public class SendMessageCommand
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public string MessageType { get; set; }

        public SendMessageCommand(Guid chatId, Guid senderId, string content, string messageType = "Text")
        {
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            MessageType = messageType;
        }
    }
}
