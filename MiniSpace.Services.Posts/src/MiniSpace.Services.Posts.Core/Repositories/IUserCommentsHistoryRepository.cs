using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserCommentsHistoryRepository
    {
        /// <summary>
        /// Saves a comment to the user's comment history.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="comment">The comment to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveCommentAsync(Guid userId, Comment comment);

        /// <summary>
        /// Retrieves all comments created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of comments.</returns>
        Task<IEnumerable<Comment>> GetUserCommentsAsync(Guid userId);

        /// <summary>
        /// Retrieves a paged response of comments created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of comments per page.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a paged response of comments.</returns>
        Task<PagedResponse<Comment>> GetUserCommentsPagedAsync(Guid userId, int pageNumber, int pageSize);

        /// <summary>
        /// Deletes a specific comment from the user's comment history.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="commentId">The ID of the comment to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteCommentAsync(Guid userId, Guid commentId);
    }
}
