using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
{
    class IdentityService : IIdentityService
    {
        private readonly IHttpClient _httpClient;
        private JwtDto JwtDto;
        public bool IsAuthenticated { get; private set; }
        
        public IdentityService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<UserDto> GetAccountAsync()
        {
            _httpClient.SetAccessToken(JwtDto.AccessToken);
            return _httpClient.GetAsync<UserDto>("identity/me");
        }

        public Task SignUpAsync(string email, string password, string role = "user", IEnumerable<string> permissions = null)
            => _httpClient.PostAsync("identity/sign-up", new {email, password, role, permissions});

        public async Task<JwtDto> SignInAsync(string email, string password)
        {
            JwtDto = await _httpClient.PostAsync<object, JwtDto>("identity/sign-in", new {email, password});
            IsAuthenticated = true;
            return JwtDto;
        }

        public void Logout()
        {
            JwtDto = null;
            IsAuthenticated = false;
        }
    }
}