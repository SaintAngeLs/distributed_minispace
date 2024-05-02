using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
{
    class IdentityService : IIdentityService
    {
        private readonly IHttpClient _httpClient;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        
        public JwtDto JwtDto { get; set; }
        public UserDto UserDto { get; set; }
        public string Name {get; private set; }
        public string Email {get; private set; }
        public bool IsAuthenticated { get; set; }
        
        public IdentityService(IHttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _jwtHandler = new JwtSecurityTokenHandler();
            _localStorage = localStorage;
             _navigationManager = navigationManager;
        }
        
        public Task<UserDto> GetAccountAsync(JwtDto jwtDto)
        {
            _httpClient.SetAccessToken(jwtDto.AccessToken);
            return _httpClient.GetAsync<UserDto>("identity/me");
        }

        public async Task<HttpResponse<object>> SignUpAsync(string firstName, string lastName, string email, string password, string role = "user",
            IEnumerable<string> permissions = null)
        {
            return await _httpClient.PostAsync<object, object>("identity/sign-up", 
                new {firstName, lastName, email, password, role, permissions});
            
        }

        // public async Task<HttpResponse<JwtDto>> SignInAsync(string email, string password)
        // {
        //     var response = await _httpClient.PostAsync<object, JwtDto>("identity/sign-in", new {email, password});
        //     JwtDto = response.Content;
            
        //     if (JwtDto != null)
        //     {
        //         var jwtToken = _jwtHandler.ReadJwtToken(JwtDto.AccessToken);
        //         var payload = jwtToken.Payload;
        //         UserDto = await GetAccountAsync(JwtDto);
        //         Name = (string)payload["name"];
        //         Email = (string)payload["e-mail"];
        //         IsAuthenticated = true;
        //     }
            
        //     return response;
        // }

         public async Task<HttpResponse<JwtDto>> SignInAsync(string email, string password)
        {
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/sign-in", new { email, password });
            if (response.Content != null)
            {
                JwtDto = response.Content;
                var jwtDtoJson = JsonSerializer.Serialize(JwtDto);
                await _localStorage.SetItemAsStringAsync("jwtDto", jwtDtoJson);

                var jwtToken = _jwtHandler.ReadJwtToken(JwtDto.AccessToken);
                var payload = jwtToken.Payload;
                UserDto = await GetAccountAsync(JwtDto);
                Name = payload.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                Email = payload.Claims.FirstOrDefault(c => c.Type == "e-mail")?.Value;
                IsAuthenticated = true;
            }
            return response;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("jwtDto");
            JwtDto = null;
            UserDto = null;
            Name = null;
            Email = null;
            IsAuthenticated = false;
            _navigationManager.NavigateTo("signin", forceLoad: true);
        }

        private async Task<JwtDto> RefreshAccessToken(string refreshToken)
        {
            var payload = new { refreshToken };
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/refresh-token", payload);
            if (response.ErrorMessage != null)
            {
                throw new InvalidOperationException($"Error refreshing token: {response.ErrorMessage.Reason}");
            }

            if (response.Content != null)
            {
                return response.Content;
            }

            throw new InvalidOperationException("Failed to refresh token");
        }

        // Make the Logout asynchronous 😕
        // public void Logout()
        // {
        //     JwtDto = null;
        //     UserDto = null;
        //     Name = null;
        //     Email = null;
        //     IsAuthenticated = false;
        // }
        public async Task<string> GetAccessTokenAsync()
        {
            var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");

            if (!string.IsNullOrEmpty(jwtDtoJson))
            {
                JwtDto jwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);

                var jwtToken = _jwtHandler.ReadJwtToken(jwtDto.AccessToken);

                if (jwtToken.ValidTo > DateTime.UtcNow)
                {
                    return jwtDto.AccessToken;
                }
                else
                {
                    if (!string.IsNullOrEmpty(jwtDto.RefreshToken))
                    {
                        try
                        {
                            JwtDto newJwtDto = await RefreshAccessToken(jwtDto.RefreshToken);

                            if (newJwtDto != null)
                            {
                                var newJwtDtoJson = JsonSerializer.Serialize(newJwtDto);

                                await _localStorage.SetItemAsStringAsync("jwtDto", newJwtDtoJson);

                                return newJwtDto.AccessToken;
                            }
                        }
                        catch (Exception ex)
                        {
                            await _localStorage.RemoveItemAsync("jwtDto");
                            
                            _navigationManager.NavigateTo("signin", forceLoad: true);

                            throw new InvalidOperationException("Failed to refresh token: " + ex.Message);
                        }
                    }

                    await _localStorage.RemoveItemAsync("jwtDto");

                    _navigationManager.NavigateTo("signin", forceLoad: true);

                    throw new InvalidOperationException("Session expired, please login again.");
                }
            }

            throw new InvalidOperationException("Authentication required.");
        }

        public async Task InitializeAuthenticationState()
        {
            var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");
            if (!string.IsNullOrEmpty(jwtDtoJson))
            {
                JwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);
                var tokenExpirationDateTime = DateTimeOffset.FromUnixTimeSeconds(JwtDto.Expires).UtcDateTime;
                if (JwtDto != null && DateTime.UtcNow < tokenExpirationDateTime)
                {
                    UserDto = await GetAccountAsync(JwtDto); 
                    IsAuthenticated = UserDto != null;
                }
                else if (JwtDto != null && !string.IsNullOrEmpty(JwtDto.RefreshToken))
                {
                    JwtDto = await RefreshAccessToken(JwtDto.RefreshToken);
                    if (JwtDto != null)
                    {
                        jwtDtoJson = JsonSerializer.Serialize(JwtDto);
                        await _localStorage.SetItemAsStringAsync("jwtDto", jwtDtoJson);
                        UserDto = await GetAccountAsync(JwtDto);
                        IsAuthenticated = UserDto != null;
                    }
                }
            }
        }

         public async Task<bool> CheckIfUserIsAuthenticated()
{
    var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");
    if (!string.IsNullOrEmpty(jwtDtoJson))
    {
        JwtDto jwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);
        var jwtToken = _jwtHandler.ReadJwtToken(jwtDto.AccessToken);
        if (jwtToken.ValidTo > DateTime.UtcNow)
        {
            IsAuthenticated = true;
        }
        else
        {
            IsAuthenticated = await TryRefreshToken(jwtDto.RefreshToken);
        }
    }
    else
    {
        IsAuthenticated = false;
    }
    return IsAuthenticated;
}



        private async Task<bool> TryRefreshToken(string refreshToken)
        {
            try
            {
                var response = await _httpClient.PostAsync<object, JwtDto>("identity/refresh-token", new { refreshToken });
                if (response.Content != null)
                {
                    var newJwtDtoJson = JsonSerializer.Serialize(response.Content);
                    await _localStorage.SetItemAsStringAsync("jwtDto", newJwtDtoJson);
                    return true;
                }
            }
            catch (Exception)
            {
                await Logout();
            }
            return false;
        }

        public async Task<bool> IsTokenValid()
        {
            var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");
            if (!string.IsNullOrEmpty(jwtDtoJson))
            {
                var jwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);
                var jwtToken = _jwtHandler.ReadJwtToken(jwtDto.AccessToken);
                return jwtToken.ValidTo > DateTime.UtcNow;
            }
            return false;
        }


    }
}