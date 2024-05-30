using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
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
