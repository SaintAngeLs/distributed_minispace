using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;

namespace MiniSpace.Web.Areas.Identity
{
    public interface IIdentityService
    {
        Task<UserDto> GetAccountAsync(string jwt);
        Task SignUpAsync(string name, string email, string password, string role = "user", IEnumerable<string> permissions = null);
        Task<JwtDto> SignInAsync(string email, string password);
    }
}