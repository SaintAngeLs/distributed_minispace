using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Comment comment);
        Task DeleteAsync(Guid id);
    }
}