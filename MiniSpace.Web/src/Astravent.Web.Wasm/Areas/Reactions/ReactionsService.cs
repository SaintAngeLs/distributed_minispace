using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Reactions.CommandDto;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Enums;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Reactions
{
    public class ReactionsService : IReactionsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;
        
        public ReactionsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }
        
        public async Task<IEnumerable<ReactionDto>> GetReactionsAsync(Guid contentId, ReactionContentType contentType)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<ReactionDto>>($"reactions?contentId={contentId}&contentType={contentType}");
        }

        public async Task<ReactionsSummaryDto> GetReactionsSummaryAsync(Guid contentId, ReactionContentType contentType)
        {
            return await _httpClient.GetAsync<ReactionsSummaryDto>($"reactions/summary?contentId={contentId}&contentType={contentType}");
        }

        public async Task<Dictionary<Guid, ReactionsSummaryDto>> GetReactionsSummariesAsync(IEnumerable<Guid> contentIds, ReactionContentType contentType)
        {
            var summaries = new Dictionary<Guid, ReactionsSummaryDto>();
            foreach (var id in contentIds)
            {
                summaries[id] = await _httpClient.GetAsync<ReactionsSummaryDto>($"reactions/summary?contentId={id}&contentType={contentType}");
            }
            return summaries;
        }


        public async Task<HttpResponse<object>> CreateReactionAsync(CreateReactionDto command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>("reactions", command);
        }

        public async Task<HttpResponse<object>> UpdateReactionAsync(UpdateReactionDto command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<object, object>($"reactions/{command.ReactionId}", command);
        }

        public async Task DeleteReactionAsync(Guid reactionId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"reactions/{reactionId}");
        }
    }    
}
