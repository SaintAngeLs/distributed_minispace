using System;
using System.Collections.Generic;
using MiniSpace.Services.Identity.Application.DTO;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface IJwtProvider
    {
        AuthDto Create(Guid userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);

        string GenerateResetToken(Guid userId);
    }
}