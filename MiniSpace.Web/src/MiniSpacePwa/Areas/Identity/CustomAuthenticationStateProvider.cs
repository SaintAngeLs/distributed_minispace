using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using MiniSpacePwa.Areas.Identity;
using System.Collections.Generic;

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
        string storedToken = await _localStorage.GetItemAsStringAsync("jwtDto");
        if (!string.IsNullOrEmpty(storedToken))
        {
            var jwtDto = JsonSerializer.Deserialize<JwtDto>(storedToken);
            if (jwtDto != null && ValidateToken(jwtDto.AccessToken))
            {
                var claims = ParseClaimsFromJwt(jwtDto.AccessToken);
                var identity = new ClaimsIdentity(claims, "jwtAuthType");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var token = _jwtHandler.ReadJwtToken(jwt);
        return token.Claims;
    }

    private bool ValidateToken(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return false;
        var token = _jwtHandler.ReadJwtToken(jwt);
        return token.ValidTo > DateTime.UtcNow;
    }

    // public async Task InitializeAsync()
    // {
    //     string storedToken = await _localStorage.GetItemAsStringAsync("jwtDto");
    //     if (!string.IsNullOrEmpty(storedToken))
    //     {
    //         var jwtDto = JsonSerializer.Deserialize<JwtDto>(storedToken);
    //         if (jwtDto != null && ValidateToken(jwtDto.AccessToken))
    //         {
    //             var claims = ParseClaimsFromJwt(jwtDto.AccessToken);
    //             var identity = new ClaimsIdentity(claims, "jwtAuthType");
    //             var user = new ClaimsPrincipal(identity);
    //             NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    //         }
    //     }
    // }

public async Task InitializeAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }


}
