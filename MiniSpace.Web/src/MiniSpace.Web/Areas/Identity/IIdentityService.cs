using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
{
    public interface IIdentityService
    { 
        public JwtDto JwtDto { get; set;}
        public UserDto UserDto { get; set; }
        bool IsAuthenticated { get; set; }
        Task<UserDto> GetAccountAsync(JwtDto jwtDto);
        Task<HttpResponse<object>> SignUpAsync(string firstName, string lastName, string email, string password, string role = "user", IEnumerable<string> permissions = null);
        Task<HttpResponse<JwtDto>> SignInAsync(string email, string password);
        Task Logout();
        Task<string> GetAccessTokenAsync();
        Task InitializeAuthenticationState();
        Task<bool> CheckIfUserIsAuthenticated();
        Task<bool> IsTokenValid();
        public Guid GetCurrentUserId();
        public string GetCurrentUserRole();
        
        Task GrantOrganizerRightsAsync(Guid userId);
        Task RevokeOrganizerRightsAsync(Guid userId);
        Task BanUserAsync(Guid userId);
        Task UnbanUserAsync(Guid userId);

        Task ForgotPasswordAsync(string email);
        Task<HttpResponse<object>> ResetPasswordAsync(string token, string email, string newPassword);
        Task<HttpResponse<object>> VerifyEmailAsync(string token, string email, string hashedToken);
    }
}