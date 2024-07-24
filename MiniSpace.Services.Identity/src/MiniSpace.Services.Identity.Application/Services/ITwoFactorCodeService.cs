using System;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface ITwoFactorCodeService
    {
        string GenerateCode(string secret);
        bool ValidateCode(string secret, string code);
    }
}
