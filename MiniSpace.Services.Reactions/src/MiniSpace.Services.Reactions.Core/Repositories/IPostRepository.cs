using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IPostRepository
    {
        Task<bool> ExistsAsync(Guid id);
    }    
}
