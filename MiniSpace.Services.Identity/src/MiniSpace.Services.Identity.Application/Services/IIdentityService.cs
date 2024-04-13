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
        Task GrantOrganizerRightsAsync(GrantOrganizerRights command);
        Task RevokeOrganizerRightsAsync(RevokeOrganizerRights command);
    }
}