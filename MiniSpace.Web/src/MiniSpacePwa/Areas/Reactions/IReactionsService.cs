using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Enums;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Reactions
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
