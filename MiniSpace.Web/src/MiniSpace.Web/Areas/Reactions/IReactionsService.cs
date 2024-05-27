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
        Task<IEnumerable<ReactionDto>> GetReactionsAsync(Guid contentId, ReactionContentType contentType);
        Task<ReactionsSummaryDto> GetReactionsSummaryAsync(Guid contentId, ReactionContentType contentType);
        Task<HttpResponse<object>> CreateReactionAsync(Guid reactionId, Guid studentId, string reactionType,
            Guid contentId, string contentType);
        Task DeleteReactionAsync(Guid reactionId);
    }    
}
