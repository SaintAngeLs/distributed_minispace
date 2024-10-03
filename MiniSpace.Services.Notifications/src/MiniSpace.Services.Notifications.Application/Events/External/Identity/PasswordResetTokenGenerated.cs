using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity
{
    [Message("identity")]
    public class PasswordResetTokenGenerated : IEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string ResetToken { get; }

        public PasswordResetTokenGenerated(Guid userId, string email, string resetToken)
        {
            UserId = userId;
            Email = email;
            ResetToken = resetToken;
        }
    }
}
