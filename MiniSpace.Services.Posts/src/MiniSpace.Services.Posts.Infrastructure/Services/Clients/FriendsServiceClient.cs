using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Services.Clients;

namespace MiniSpace.Services.Posts.Infrastructure.Services.Clients
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

        public Task<IEnumerable<UserFriendsDto>> GetAsync(Guid userId)
            => _httpClient.GetAsync<IEnumerable<UserFriendsDto>>($"{_url}/friends/{userId}");
    }
}