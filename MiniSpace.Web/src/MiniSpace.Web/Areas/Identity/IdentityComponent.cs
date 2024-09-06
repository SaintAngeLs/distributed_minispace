using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
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
        
        public Task<HttpResponse<JwtDto>> SignInAsync(string email, string password, string deviceType)
            => _identityService.SignInAsync(email, password, deviceType);

        public Task<UserDto> GetAccount(JwtDto jwtDto)
            => _identityService.GetAccountAsync(jwtDto);
    }
}