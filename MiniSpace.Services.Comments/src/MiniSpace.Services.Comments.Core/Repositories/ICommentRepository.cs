using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetAsync(Guid id);
        Task<IEnumerable<Comment>> GetToUpdateAsync();
        Task AddAsync(Comment post);
        Task UpdateAsync(Comment post);
        Task DeleteAsync(Guid id);
    }    
}