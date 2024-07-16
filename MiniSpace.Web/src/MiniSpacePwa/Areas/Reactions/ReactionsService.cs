using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.Areas.Identity;
using MiniSpacePwa.DTO;
using MiniSpacePwa.DTO.Enums;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Reactions
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
        
        public Task<IEnumerable<ReactionDto>> GetReactionsAsync(Guid contentId, ReactionContentType contentType)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<ReactionDto>>($"reactions?contentId={contentId}&contentType={contentType}");
        }

        public Task<ReactionsSummaryDto> GetReactionsSummaryAsync(Guid contentId, ReactionContentType contentType)
        {
            return _httpClient.GetAsync<ReactionsSummaryDto>($"reactions/summary?contentId={contentId}&contentType={contentType}");
        }

        public Task<HttpResponse<object>> CreateReactionAsync(Guid reactionId, Guid studentId, string reactionType,
            Guid contentId, string contentType)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("reactions",
                new { reactionId, studentId, reactionType, contentId, contentType });
        }

        public Task DeleteReactionAsync(Guid reactionId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"reactions/{reactionId}");
        }
    }    
}
