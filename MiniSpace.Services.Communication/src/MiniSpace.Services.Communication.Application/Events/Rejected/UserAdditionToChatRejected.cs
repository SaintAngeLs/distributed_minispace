using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Communication.Application.Events.Rejected
{
    public class UserAdditionToChatRejected : IRejectedEvent
    {
        public Guid ChatId { get; }
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        public UserAdditionToChatRejected(Guid chatId, Guid userId, string reason, string code)
        {
            ChatId = chatId;
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
