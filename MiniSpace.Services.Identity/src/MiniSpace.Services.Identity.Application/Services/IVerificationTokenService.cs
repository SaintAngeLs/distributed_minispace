using System;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface IVerificationTokenService
    {
        (string Token, string HashedToken) GenerateToken(Guid userId, string email);
        bool ValidateToken(string token, string hashedToken);
    }
}
