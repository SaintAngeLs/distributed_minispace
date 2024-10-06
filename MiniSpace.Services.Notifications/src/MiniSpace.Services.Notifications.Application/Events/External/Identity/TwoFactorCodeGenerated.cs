using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity
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
