using System.Threading.Tasks;
using MiniSpace.Web.DTO;

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

        public Task SignUpAsync(string email, string password, string role = "user")
            => _identityService.SignUpAsync(email, password, role);
        
        public Task<JwtDto> SignInAsync(string email, string password)
            => _identityService.SignInAsync(email, password);

        public Task<UserDto> GetAccount()
            => _identityService.GetAccountAsync();
    }
}