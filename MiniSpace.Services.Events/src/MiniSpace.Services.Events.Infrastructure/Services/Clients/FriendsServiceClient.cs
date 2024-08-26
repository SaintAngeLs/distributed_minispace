using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services.Clients;

namespace MiniSpace.Services.Events.Infrastructure.Services.Clients
{
    [ExcludeFromCodeCoverage]
    public class FriendsServiceClient : IFriendsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public FriendsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["friends"];
        }

        public async Task<IEnumerable<UserFriendsDto>> GetAsync(Guid studentId)
        {
            var pagedResponse = await _httpClient.GetAsync<MiniSpace.Services.Events.Application.DTO.PagedResult<UserFriendsDto>>($"{_url}/friends/{studentId}");
            return pagedResponse.Items;
        }
    }
}