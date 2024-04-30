using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Friends
{
    public class FriendsService : IFriendsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public FriendDto FriendDto { get; private set; }
        
        public FriendsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task UpdateFriendDto(Guid friendId)
        {
            FriendDto = await GetFriendAsync(friendId);
        }

        public void ClearFriendDto()
        {
            FriendDto = null;
        }
        
        public Task<FriendDto> GetFriendAsync(Guid friendId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<FriendDto>($"friends/{friendId}");
        }

        public Task<IEnumerable<FriendDto>> GetAllFriendsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<FriendDto>>("friends");
        }

        public Task<HttpResponse<object>> AddFriendAsync(Guid friendId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PostAsync<object, object>("friends", new { friendId });
        }

        public Task RemoveFriendAsync(Guid friendId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.DeleteAsync($"friends/{friendId}");
        }

        public Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<StudentDto>>("students");
        }
    }    
}
