using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Convey.Auth;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Infrastructure.Auth
{
    [ExcludeFromCodeCoverage]
    public class JwtProvider : IJwtProvider
    {
        private readonly IJwtHandler _jwtHandler;

        public JwtProvider(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public AuthDto Create(Guid userId, Role role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
        {
            var roleString = role.ToString();
            var jwt = _jwtHandler.CreateToken(userId.ToString("N"), roleString, audience, claims);

            return new AuthDto
            {
                AccessToken = jwt.AccessToken,
                Role = jwt.Role,
                Expires = jwt.Expires
            };
        }

        public string GenerateResetToken(Guid userId)
        {
            // Generating a token that might be used specifically as a reset token
            // The implementation specifics would depend on your application's security requirements
            var claims = new Dictionary<string, IEnumerable<string>> {
                // Additional claims can be added here if needed
            };
            var jwt = _jwtHandler.CreateToken(userId.ToString("N"), "ResetToken", null, claims);

            return jwt.AccessToken;
        }
    }
}
