using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Communication.Application.Commands
{
    public class UpdateMessageStatus : ICommand
    {
        public Guid ChatId { get; set; }
        public Guid MessageId { get; set; }
        public string Status { get; set; }

        public UpdateMessageStatus(Guid chatId, Guid messageId, string status)
        {
            ChatId = chatId;
            MessageId = messageId;
            Status = status;
        }
    }
}
