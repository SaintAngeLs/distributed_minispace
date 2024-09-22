using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Identity
{
    public class IdentityComponent
    {
        private readonly IIdentityService _identityService;

        public IdentityComponent(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task OnInit()
        {
            return Task.CompletedTask;
        }

        public Task SignUpAsync(string firstName, string lastName, string email, string password, string role = "user")
            => _identityService.SignUpAsync(firstName, lastName, email, password, role);
        
        public Task<HttpResponse<JwtDto>> SignInAsync(string email, string password, string deviceType, string ipAddress)
            => _identityService.SignInAsync(email, password, deviceType, ipAddress); 

        public Task<UserDto> GetAccount(JwtDto jwtDto)
            => _identityService.GetAccountAsync(jwtDto);
    }
}