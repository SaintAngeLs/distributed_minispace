using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
{
    var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");
    if (string.IsNullOrEmpty(jwtDtoJson))
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    JwtDto jwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);
    var token = new JwtSecurityToken(jwtDto.AccessToken);
    if (token.ValidTo < DateTime.UtcNow)
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    var identity = new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.Name, "Name"),
        new Claim(ClaimTypes.Email, "email"),
    }, "apiauth_type");

    return new AuthenticationState(new ClaimsPrincipal(identity));
}


}
