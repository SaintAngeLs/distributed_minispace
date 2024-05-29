using System;

namespace MiniSpace.Services.Identity.Core.Entities
{
    public class UserResetToken : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public string ResetToken { get; private set; }
        public DateTime? ResetTokenExpiration { get; private set; }

        public UserResetToken(Guid userId, string resetToken, DateTime resetTokenExpiration)
        {
            UserId = userId;
            ResetToken = resetToken;
            ResetTokenExpiration = resetTokenExpiration;
        }

        public void SetResetToken(string token, DateTime expiration)
        {
            ResetToken = token;
            ResetTokenExpiration = expiration;
        }

        public void ClearResetToken()
        {
            ResetToken = null;
            ResetTokenExpiration = null;
        }

        public bool ResetTokenIsValid(string token)
        {
            return ResetToken == token && ResetTokenExpiration.HasValue && ResetTokenExpiration > DateTime.UtcNow;
        }
    }
}
