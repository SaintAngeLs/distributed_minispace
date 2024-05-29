using System;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class UserResetTokenDto
    {
        public Guid UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }

        public UserResetTokenDto()
        {
        }

        public UserResetTokenDto(UserResetToken userResetToken)
        {
            UserId = userResetToken.UserId;
            ResetToken = userResetToken.ResetToken;
            ResetTokenExpiration = userResetToken.ResetTokenExpiration;
        }
    }
}
