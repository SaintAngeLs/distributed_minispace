using System.Threading.Tasks;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}