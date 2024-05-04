using MiniSpace.Services.Comments.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    }    
}