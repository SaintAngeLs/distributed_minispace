namespace MiniSpace.Services.Identity.Application.Services
{
    public interface ITwoFactorSecretTokenService
    {
        string GenerateSecret();
    }
}
