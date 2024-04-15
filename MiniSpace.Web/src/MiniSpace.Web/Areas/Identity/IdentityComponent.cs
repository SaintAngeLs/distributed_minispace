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

        public Task SignUpAsync(string firstName, string lastName, string email, string password, string role = "user")
            => _identityService.SignUpAsync(firstName, lastName, email, password, role);
        
        public Task<JwtDto> SignInAsync(string email, string password)
            => _identityService.SignInAsync(email, password);

        public Task<UserDto> GetAccount()
            => _identityService.GetAccountAsync();
    }
}