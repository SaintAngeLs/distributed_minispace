using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MiniSpace.Services.Identity.Application.Services;

namespace MiniSpace.Services.Identity.Infrastructure.Auth
{
    public class VerificationTokenService : IVerificationTokenService
    {
        private readonly IPasswordHasher<IVerificationTokenService> _passwordHasher;

        public VerificationTokenService(IPasswordHasher<IVerificationTokenService> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public (string Token, string HashedToken) GenerateToken(Guid userId, string email)
        {
            var token = GenerateTokenString(userId, email);
            var hashedToken = _passwordHasher.HashPassword(this, token);
            return (token, hashedToken);
        }

        public bool ValidateToken(string token, string hashedToken)
        {
            return _passwordHasher.VerifyHashedPassword(this, hashedToken, token) != PasswordVerificationResult.Failed;
        }

        private string GenerateTokenString(Guid userId, string email)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                var token = Convert.ToBase64String(tokenData);
                var combined = $"{userId}{email}{token}";
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(combined));
            }
        }
    }
}
