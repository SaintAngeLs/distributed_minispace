using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IPostRepository
    {
        //Task<Post> GetAsync(Guid id);
        //Task<IEnumerable<Post>> GetToUpdateAsync();
        //Task AddAsync(Post post);
        //Task UpdateAsync(Post post);
        Task DeleteAsync(Guid id);
    }    
}
