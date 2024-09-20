using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserCommentsHistoryRepository
    {
        Task SaveCommentAsync(Guid userId, Comment comment);

        Task<IEnumerable<Comment>> GetUserCommentsAsync(Guid userId);

        Task<PagedResponse<Comment>> GetUserCommentsPagedAsync(Guid userId, int pageNumber, int pageSize);

        Task DeleteCommentAsync(Guid userId, Guid commentId);
    }
}
