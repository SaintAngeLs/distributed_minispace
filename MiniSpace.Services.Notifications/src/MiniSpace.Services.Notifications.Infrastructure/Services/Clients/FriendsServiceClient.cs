using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
{
    public class FriendsServiceClient :  IFriendsServiceClient
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
        public Task<IEnumerable<FriendDto>> GetFriendsAsync(Guid studentId)
            => _httpClient.GetAsync<IEnumerable<FriendDto>>($"{_url}/friends/{studentId}");
        public Task<IEnumerable<FriendRequestDto>> GetRequestsAsync(Guid studentId)
            => _httpClient.GetAsync<IEnumerable<FriendRequestDto>>($"{_url}/friends/requests/{studentId}");

    }
}