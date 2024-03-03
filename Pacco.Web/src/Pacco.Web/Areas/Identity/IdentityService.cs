using System.Threading.Tasks;
using Pacco.Web.DTO;
using Pacco.Web.HttpClients;

namespace Pacco.Web.Areas.Identity
{
    class IdentityService : IIdentityService
    {
        private readonly IHttpClient _httpClient;

        public IdentityService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<UserDto> GetAccountAsync(string jwt)
        {
            _httpClient.SetAccessToken(jwt);
            return _httpClient.GetAsync<UserDto>("identity/me");
        }

        public Task SignUpAsync(string email, string password, string role = "user")
            => _httpClient.PostAsync("identity/sign-up", new {email, password, role});

        public Task<JwtDto> SignInAsync(string email, string password)
            => _httpClient.PostAsync<object, JwtDto>("identity/sign-in", new {email, password});
    }
}