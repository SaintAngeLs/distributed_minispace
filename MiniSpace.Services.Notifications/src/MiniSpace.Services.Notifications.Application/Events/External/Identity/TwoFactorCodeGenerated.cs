using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
    [Message("identity")]
    public class TwoFactorCodeGenerated : IEvent
    {
        public Guid UserId { get; }
        public string Code { get; }

        public TwoFactorCodeGenerated(Guid userId, string code)
        {
            UserId = userId;
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
