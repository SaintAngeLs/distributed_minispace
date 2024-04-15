using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;

namespace MiniSpace.Web.Areas.Identity
{
    public interface IIdentityService
    {
        public JwtDto JwtDto { get; }
        bool IsAuthenticated { get; }
        Task<UserDto> GetAccountAsync();
        Task SignUpAsync(string email, string password, string role = "user", IEnumerable<string> permissions = null);
        Task<JwtDto> SignInAsync(string email, string password);
        void Logout();
    }
}