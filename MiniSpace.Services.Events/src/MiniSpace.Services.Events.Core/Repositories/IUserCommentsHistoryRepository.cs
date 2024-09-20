using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IUserCommentsHistoryRepository
    {
        Task SaveCommentAsync(Guid userId, Comment comment);

        Task<IEnumerable<Comment>> GetUserCommentsAsync(Guid userId);

        Task<PagedResponse<Comment>> GetUserCommentsPagedAsync(Guid userId, int pageNumber, int pageSize);

        Task DeleteCommentAsync(Guid userId, Guid commentId);
    }
}
