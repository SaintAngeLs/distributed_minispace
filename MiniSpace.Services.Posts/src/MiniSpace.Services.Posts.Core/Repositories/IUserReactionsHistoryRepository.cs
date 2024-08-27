using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IUserReactionsHistoryRepository
    {
        /// <summary>
        /// Saves a reaction to the user's reaction history.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="reaction">The reaction to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveReactionAsync(Guid userId, Reaction reaction);

        /// <summary>
        /// Retrieves all reactions created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a list of reactions.</returns>
        Task<IEnumerable<Reaction>> GetUserReactionsAsync(Guid userId);

        /// <summary>
        /// Retrieves a paged response of reactions created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of reactions per page.</param>
        /// <returns>A task representing the asynchronous operation, with a result of a paged response of reactions.</returns>
        Task<PagedResponse<Reaction>> GetUserReactionsPagedAsync(Guid userId, int pageNumber, int pageSize);

        /// <summary>
        /// Deletes a specific reaction from the user's reaction history.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="reactionId">The ID of the reaction to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteReactionAsync(Guid userId, Guid reactionId);
    }
}
