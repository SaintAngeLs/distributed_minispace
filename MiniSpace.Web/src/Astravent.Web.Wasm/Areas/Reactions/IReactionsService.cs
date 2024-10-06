using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Reactions.CommandDto;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Enums;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Reactions
{
    public interface IReactionsService
    {
        Task<IEnumerable<ReactionDto>> GetReactionsAsync(Guid contentId, ReactionContentType contentType);
        Task<ReactionsSummaryDto> GetReactionsSummaryAsync(Guid contentId, ReactionContentType contentType);
        Task<Dictionary<Guid, ReactionsSummaryDto>> GetReactionsSummariesAsync(IEnumerable<Guid> contentIds, ReactionContentType contentType);
        Task<HttpResponse<object>> CreateReactionAsync(CreateReactionDto command);
        Task<HttpResponse<object>> UpdateReactionAsync(UpdateReactionDto command);
        Task DeleteReactionAsync(Guid reactionId);
    }    
}
