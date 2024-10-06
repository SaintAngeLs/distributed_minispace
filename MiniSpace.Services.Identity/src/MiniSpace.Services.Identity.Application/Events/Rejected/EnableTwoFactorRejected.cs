using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events.Rejected
{
    
    public class EnableTwoFactorRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        public EnableTwoFactorRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
