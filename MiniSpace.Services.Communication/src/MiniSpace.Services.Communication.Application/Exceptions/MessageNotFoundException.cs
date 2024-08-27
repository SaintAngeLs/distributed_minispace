using System;

namespace MiniSpace.Services.Communication.Application.Exceptions
{
    public class MessageNotFoundException : AppException
    {
        public override string Code { get; } = "message_not_found";

        public Guid MessageId { get; }

        public MessageNotFoundException(Guid messageId)
            : base($"Message with ID '{messageId}' was not found.")
        {
            MessageId = messageId;
        }
    }
}
