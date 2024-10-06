using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Identity.Application.Events
{
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
