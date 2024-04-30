using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
{
    class IdentityService : IIdentityService
    {
        private readonly IHttpClient _httpClient;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        public JwtDto JwtDto { get; private set; }
        public UserDto UserDto { get; private set; }
        public string Name {get; private set; }
        public string Email {get; private set; }
        public bool IsAuthenticated { get; private set; }
        
        public IdentityService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
            _jwtHandler = new JwtSecurityTokenHandler();
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

        public async Task<HttpResponse<JwtDto>> SignInAsync(string email, string password)
        {
            var response = await _httpClient.PostAsync<object, JwtDto>("identity/sign-in", new {email, password});
            JwtDto = response.Content;
            
            if (JwtDto != null)
            {
                var jwtToken = _jwtHandler.ReadJwtToken(JwtDto.AccessToken);
                var payload = jwtToken.Payload;
                UserDto = await GetAccountAsync(JwtDto);
                Name = (string)payload["name"];
                Email = (string)payload["e-mail"];
                IsAuthenticated = true;
            }
            
            return response;
        }

        public void Logout()
        {
            JwtDto = null;
            UserDto = null;
            Name = null;
            Email = null;
            IsAuthenticated = false;
        }

        public Task GrantOrganizerRights(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.PostAsync($"identity/users/{userId}/organizer-rights", new {userId});
        }

        public Task RevokeOrganizerRights(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"identity/users/{userId}/organizer-rights");
        }

        public Task BanUser(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.PostAsync($"identity/users/{userId}/ban", new {userId});
        }

        public Task UnbanUser(Guid userId)
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"identity/users/{userId}/ban");
        }
    }
}