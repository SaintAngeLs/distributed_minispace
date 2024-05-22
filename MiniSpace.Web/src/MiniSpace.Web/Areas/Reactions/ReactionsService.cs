using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Reactions
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
        
        public Task<IEnumerable<ReactionDto>> GetReactions(Guid contentId, ReactionContentType contentType)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<ReactionDto>>($"reactions?contentId={contentId}&contentType={contentType}");
        }

        public Task<ReactionsSummaryDto> GetReactionsSummary(Guid contentId, ReactionContentType contentType)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<ReactionsSummaryDto>($"reactions/summary?contentId={contentId}&contentType={contentType}");
        }

        public Task<HttpResponse<object>> CreateReaction(Guid reactionId, Guid studentId, string reactionType,
            Guid contentId, string contentType)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("reactions",
                new { reactionId, studentId, reactionType, contentId, contentType });
        }

        public Task DeleteReaction(Guid reactionId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"reactions/{reactionId}");
        }
    }    
}
