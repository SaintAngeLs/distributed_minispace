using System;

namespace MiniSpace.Services.Identity.Core.Entities
{
    public class UserResetToken : AggregateRoot
    {
        public Guid UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        public UserResetToken(Guid userId, string resetToken, DateTime resetTokenExpiration)
        {
            UserId = userId;
            ResetToken = resetToken;
            ResetTokenExpires = resetTokenExpiration;
        }

        public void SetResetToken(string token, DateTime expiration)
        {
            ResetToken = token;
            ResetTokenExpires = expiration;
        }

        public void ClearResetToken()
        {
            ResetToken = null;
            ResetTokenExpires = null;
        }

        public bool ResetTokenIsValid(string token)
        {
            return ResetToken == token && ResetTokenExpires.HasValue && ResetTokenExpires > DateTime.UtcNow;
        }
    }
}
