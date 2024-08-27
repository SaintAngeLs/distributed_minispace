using System;

namespace MiniSpace.Services.Communication.Application.Exceptions
{
    public class ChatNotFoundException : AppException
    {
        public override string Code { get; } = "chat_not_found";

        public Guid ChatId { get; }

        public ChatNotFoundException(Guid chatId)
            : base($"Chat with ID '{chatId}' was not found.")
        {
            ChatId = chatId;
        }
    }
}
