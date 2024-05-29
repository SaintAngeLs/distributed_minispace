using System;
using System.Threading.Tasks;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Core.Repositories
{
    public interface IUserResetTokenRepository
    {
        Task SaveAsync(UserResetToken userResetToken);
        Task<UserResetToken> GetByUserIdAsync(Guid userId);
        Task InvalidateTokenAsync(Guid userId);
    }
}
