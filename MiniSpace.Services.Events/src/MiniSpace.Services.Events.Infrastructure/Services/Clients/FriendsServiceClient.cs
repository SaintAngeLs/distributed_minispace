using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services.Clients;

namespace MiniSpace.Services.Events.Infrastructure.Services.Clients
{
    public class FriendsServiceClient : IFriendsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public FriendsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["friends"];
        }

        public Task<IEnumerable<FriendDto>> GetAsync(Guid studentId)
            => _httpClient.GetAsync<IEnumerable<FriendDto>>($"{_url}/friends/{studentId}");

    }
}