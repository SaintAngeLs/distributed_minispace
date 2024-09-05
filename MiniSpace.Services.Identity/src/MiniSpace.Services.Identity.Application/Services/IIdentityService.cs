using System;
using System.Threading.Tasks;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Application.DTO;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface IIdentityService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<AuthDto> SignInAsync(SignIn command);
        Task SignUpAsync(SignUp command);
        Task BanUserAsync(BanUser command);
        Task UnbanUserAsync(UnbanUser command);
        Task ForgotPasswordAsync(ForgotPassword command);
        Task ResetPasswordAsync(ResetPassword command);

        Task VerifyEmailAsync(VerifyEmail command);
        Task EnableTwoFactorAsync(EnableTwoFactor command);
        Task DisableTwoFactorAsync(DisableTwoFactor command);
        Task<string> GenerateTwoFactorSecretAsync(GenerateTwoFactorSecret command);
        Task<AuthDto> VerifyTwoFactorCodeAsync(VerifyTwoFactorCode command);
        Task UpdateUserStatusAsync(UpdateUserStatus command);
    }
}