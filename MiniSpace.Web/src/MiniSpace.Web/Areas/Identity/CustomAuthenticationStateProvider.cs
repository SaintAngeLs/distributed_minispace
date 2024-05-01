using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using MiniSpace.Web.Areas.Identity;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
    private readonly IIdentityService _identityService;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IIdentityService identityService)
    {
        _localStorage = localStorage;
        _identityService = identityService;
        _identityService.InitializeAuthenticationState().ConfigureAwait(false);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await _identityService.InitializeAuthenticationState();

        if (!_identityService.IsAuthenticated)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, _identityService.UserDto.Name),
            new Claim(ClaimTypes.Email, _identityService.UserDto.Email),
        }, "apiauth_type");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

}
