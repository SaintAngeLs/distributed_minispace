using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External
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
