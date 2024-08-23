using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Comments.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetAsync(Guid id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Comment>> GetByEventIdAsync(Guid eventId);
        Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
        Task<PagedResponse<Comment>> BrowseCommentsAsync(BrowseCommentsRequest request);
    }
}
