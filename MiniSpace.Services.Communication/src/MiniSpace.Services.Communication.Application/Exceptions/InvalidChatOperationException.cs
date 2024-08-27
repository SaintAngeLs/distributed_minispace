using System;

namespace MiniSpace.Services.Communication.Application.Exceptions
{
    public class InvalidChatOperationException : AppException
    {
        public override string Code { get; } = "invalid_chat_operation";

        public InvalidChatOperationException(string message)
            : base(message)
        {
        }
    }
}
