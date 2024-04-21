using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionRepository
    {
        //Task<Reaction> GetAsync(Guid id);
        //Task<IEnumerable<Reaction>> GetToUpdateAsync();
        Task AddAsync(Reaction reaction);
        //Task UpdateAsync(Reaction reaction);
        //Task DeleteAsync(Guid id);
    }    
}
