using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Identity
{
    public interface IIdentityService
    { 
        public JwtDto JwtDto { get; }
        bool IsAuthenticated { get; }
        Task<UserDto> GetAccountAsync();
        Task SignUpAsync(string firstName, string lastName, string email, string password, string role = "user", IEnumerable<string> permissions = null);
        Task<HttpResponse<JwtDto>> SignInAsync(string email, string password);
        void Logout();
    }
}