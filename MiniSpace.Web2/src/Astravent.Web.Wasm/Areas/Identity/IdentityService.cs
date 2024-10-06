using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Identity
{
    class IdentityService : IIdentityService
    {
        private readonly IHttpClient _httpClient;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        
        public JwtDto JwtDto { get; set; }
        public UserDto UserDto { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool IsAuthenticated { get; set; }
        
        public IdentityService(IHttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _jwtHandler = new JwtSecurityTokenHandler();
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }
        
        public async Task<UserDto> GetAccountAsync(JwtDto jwtDto)
        {
            if (jwtDto == null || string.IsNullOrEmpty(jwtDto.AccessToken))
                throw new ArgumentNullException(nameof(jwtDto), "JWT DTO or Access Token is null");

            _httpClient.SetAccessToken(jwtDto.AccessToken);
            var response = _httpClient.GetAsync<UserDto>("identity/me");
            Console.WriteLine($"GetAccountAsync response: {response}");
            return await response;
        }

        public async Task<HttpResponse<object>> SignUpAsync(string firstName, string lastName, string email, string password, string role = "user",
            IEnumerable<string> permissions = null)
        {
            return await _httpClient.PostAsync<object, object>("identity/sign-up", 
                new { firstName, lastName, email, password, role, permissions });
        }

        public async Task<HttpResponse<JwtDto>> SignInAsync(string email, string password, string deviceType)
        {
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/sign-in", 
                new { email, password, deviceType });

            if (response.Content != null)
            {
                JwtDto = response.Content;

                if (JwtDto.IsTwoFactorRequired)
                {
                    return response;
                }

                var jwtDtoJson = JsonSerializer.Serialize(JwtDto);
                await _localStorage.SetItemAsStringAsync("jwtDto", jwtDtoJson);

                var jwtToken = _jwtHandler.ReadJwtToken(JwtDto.AccessToken);
                var payload = jwtToken.Payload;
                UserDto = await GetAccountAsync(JwtDto);
                Name = payload.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                Email = payload.Claims.FirstOrDefault(c => c.Type == "e-mail")?.Value;
                IsAuthenticated = true;
            }
            else
            {
                throw new InvalidOperationException("Failed to sign in. No JWT token received.");
            }
            return response;
        }

        public async Task Logout()
        {
            if (JwtDto != null && !string.IsNullOrEmpty(JwtDto.RefreshToken))
            {
                await RevokeRefreshToken(JwtDto.RefreshToken);
            }
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
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/refresh-tokens/use", payload);
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

        private async Task RevokeRefreshToken(string refreshToken)
        {
            var payload = new { refreshToken };
            var response = await _httpClient.PostAsync<object, object>("identity/refresh-tokens/revoke", payload);
            Console.WriteLine($"Refresh token revocation response: {response}");
            if (response.ErrorMessage != null)
            {
                Console.WriteLine($"Error revoking refresh token: {response.ErrorMessage.Reason}");
                throw new InvalidOperationException($"Error revoking refresh token: {response.ErrorMessage.Reason}");
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var jwtDtoJson = await _localStorage.GetItemAsStringAsync("jwtDto");

            if (!string.IsNullOrEmpty(jwtDtoJson))
            {
                JwtDto jwtDto = JsonSerializer.Deserialize<JwtDto>(jwtDtoJson);

                if (jwtDto != null && !string.IsNullOrEmpty(jwtDto.AccessToken))
                {
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
                if (jwtDto?.AccessToken != null)
                {
                    try
                    {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Token validation error: {ex.Message}");
                        IsAuthenticated = false;
                    }
                }
                else
                {
                    Console.WriteLine("AccessToken is null");
                    IsAuthenticated = false;
                }
            }
            else
            {
                Console.WriteLine("jwtDtoJson is null or empty");
                IsAuthenticated = false;
            }
            return IsAuthenticated;
        }

        private async Task<bool> TryRefreshToken(string refreshToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var response = await _httpClient.PostAsync<object, JwtDto>("identity/refresh-tokens/use", new { refreshToken });
                    if (response.Content != null)
                    {
                        var newJwtDtoJson = JsonSerializer.Serialize(response.Content);
                        await _localStorage.SetItemAsStringAsync("jwtDto", newJwtDtoJson);
                        JwtDto = response.Content;
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("RefreshToken is null or empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing token: {ex.Message}");
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
                if (jwtDto != null && !string.IsNullOrEmpty(jwtDto.AccessToken))
                {
                    var jwtToken = _jwtHandler.ReadJwtToken(jwtDto.AccessToken);
                    return jwtToken.ValidTo > DateTime.UtcNow;
                }
                else
                {
                    Console.WriteLine("jwtDto or AccessToken is null");
                }
            }
            else
            {
                Console.WriteLine("jwtDtoJson is null or empty");
            }
            return false;
        }
        
        public Guid GetCurrentUserId()
        {
            if (UserDto != null && UserDto.Id != Guid.Empty)
            {
                return UserDto.Id;
            }
            throw new InvalidOperationException("No user is currently logged in.");
        }

        public string GetCurrentUserRole()
        {
            if (UserDto != null && UserDto.Id != Guid.Empty)
            {
                return UserDto.Role;
            }
            return string.Empty;
        }
        
        public Task GrantOrganizerRightsAsync(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.PostAsync($"identity/users/{userId}/organizer-rights", new { userId });
        }

        public Task RevokeOrganizerRightsAsync(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"identity/users/{userId}/organizer-rights");
        }

        public Task BanUserAsync(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.PostAsync($"identity/users/{userId}/ban", new { userId });
        }

        public Task UnbanUserAsync(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"identity/users/{userId}/ban");
        }

        public async Task ForgotPasswordAsync(string email)
        {
            await _httpClient.PostAsync<object>("identity/password/forgot", new { Email = email });
        }

        public async Task<HttpResponse<object>> ResetPasswordAsync(string token, string email, string newPassword)
        {
            Console.WriteLine($"Attempting to reset password with the following parameters:");
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"NewPassword: {newPassword}");

            try
            {
                // Decode token to extract user ID (pseudo-code, replace with your actual token decoding logic)
                var userId = DecodeToken(token);
                Console.WriteLine($"Decoded UserId: {userId}");

                // Proceed with password reset
                var response = await _httpClient.PostAsync<object, object>("identity/password/reset", new { UserId = userId, Token = token, Email = email, NewPassword = newPassword });
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password reset: {ex.Message}");
                throw; // Rethrow the exception after logging
            }
        }

        private Guid DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub"); 
            return Guid.Parse(userIdClaim.Value);
        }

        public async Task<HttpResponse<object>> VerifyEmailAsync(string token, string email, string hashedToken)
        {
            var response = await _httpClient.PostAsync<object, object>("identity/email/verify", new { Token = token, Email = email, HashedToken = hashedToken });
            return response;
        }

        public async Task<string> GenerateTwoFactorSecretAsync(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            var response = await _httpClient.PostAsync<object, GenerateTwoFactorSecretResponse>("identity/2fa/generate-secret", new { UserId = userId });
            if (response.Content != null)
            {
                return response.Content.Secret;
            }
            throw new InvalidOperationException("Failed to generate two-factor secret.");
        }

        public async Task EnableTwoFactorAsync(Guid userId, string secret)
        {
            await _httpClient.PostAsync("identity/2fa/enable", new { UserId = userId, Secret = secret });
        }

        public async Task DisableTwoFactorAsync(Guid userId)
        {
            await _httpClient.PostAsync("identity/2fa/disable", new { UserId = userId });
        }

        public async Task<HttpResponse<JwtDto>> VerifyTwoFactorCodeAsync(Guid userId, string code, string deviceType)
        {
            var payload = new { userId, code, deviceType };
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/2fa/verify-code", payload);
            if (response.Content != null)
            {
                JwtDto = response.Content;
                var jwtDtoJson = JsonSerializer.Serialize(JwtDto);
                await _localStorage.SetItemAsStringAsync("jwtDto", jwtDtoJson);

                var jwtToken = _jwtHandler.ReadJwtToken(JwtDto.AccessToken);
                var payloadClaims = jwtToken.Payload;
                UserDto = await GetAccountAsync(JwtDto);
                Name = payloadClaims.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                Email = payloadClaims.Claims.FirstOrDefault(c => c.Type == "e-mail")?.Value;
                IsAuthenticated = true;
            }
            return response;
        }

        public async Task<HttpResponse<object>> UpdateStatus(Guid userId, bool isOnline, string deviceType)
        {
            var payload = new { userId, isOnline, deviceType};
            
            var response = await _httpClient.PutAsync<object, object>("identity/users/status", payload);

            if (response.ErrorMessage != null)
            {
                throw new InvalidOperationException($"Error updating user status: {response.ErrorMessage.Reason}");
            }

            return response;
        }

    }
}
