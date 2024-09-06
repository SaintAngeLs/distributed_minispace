using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class SendMessage : ICommand
    {
        public Guid ChatId { get; }
        public Guid SenderId { get; }
        public string Content { get; }
        public string MessageType { get; }

        public SendMessage(Guid chatId, Guid senderId, string content, string messageType = "Text")
        {
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            MessageType = messageType;
        }
    }
}
