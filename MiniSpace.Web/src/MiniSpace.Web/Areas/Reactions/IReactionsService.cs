using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Reactions
{
    public interface IReactionsService
    {
        Task<IEnumerable<ReactionDto>> GetReactions(Guid contentId, ReactionContentType contentType);
        Task<ReactionsSummaryDto> GetReactionsSummary(Guid contentId, ReactionContentType contentType);
        Task<HttpResponse<object>> CreateReaction(Guid reactionId, Guid studentId, string reactionType,
            Guid contentId, string contentType);
        Task DeleteReaction(Guid reactionId);
    }    
}
