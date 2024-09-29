using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Astravent.Web.Wasm.Areas.Identity;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.Areas.Identity
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
        private readonly IIdentityService _identityService;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IIdentityService identityService)
        {
            _localStorage = localStorage;
            _identityService = identityService;
            InitializeAsync();
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
                else if (jwtDto != null && !string.IsNullOrEmpty(jwtDto.RefreshToken))
                {
                    try
                    {
                        var newJwtDto = await _identityService.RefreshAccessToken(jwtDto.RefreshToken);
                        if (newJwtDto != null)
                        {
                            var newJwtDtoJson = JsonSerializer.Serialize(newJwtDto);
                            await _localStorage.SetItemAsStringAsync("jwtDto", newJwtDtoJson);
                            var claims = ParseClaimsFromJwt(newJwtDto.AccessToken);
                            var identity = new ClaimsIdentity(claims, "jwtAuthType");
                            var user = new ClaimsPrincipal(identity);
                            return new AuthenticationState(user);
                        }
                    }
                    catch
                    {
                        await _localStorage.RemoveItemAsync("jwtDto");
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }
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

        private async Task InitializeAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
    }
}