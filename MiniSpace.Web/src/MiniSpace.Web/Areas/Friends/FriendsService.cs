using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Application.Dto;
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
        
        public async Task<FriendDto> GetFriendAsync(Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<FriendDto>($"friends/{friendId}");
        }

        public async Task<IEnumerable<FriendDto>> GetAllFriendsAsync()
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<FriendDto>>("friends");
        }


         public async Task<HttpResponse<object>> AddFriendAsync(Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<object, object>("friends", new { friendId });
        }

        public async Task RemoveFriendAsync(Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"friends/{friendId}");
        }


        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            if (_httpClient == null) throw new InvalidOperationException("HTTP client is not initialized.");
            string accessToken = await _identityService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Invalid or missing access token.");

            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<StudentDto>>("students");
        }

        public async Task<StudentDto> GetStudentAsync(Guid studentId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentDto>($"students/{studentId}");
        }

        public async Task InviteStudent(Guid inviterId, Guid inviteeId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            var payload = new { inviterId = inviterId, inviteeId = inviteeId };
            await _httpClient.PostAsync<object, HttpResponse<object>>($"friends/{inviteeId}/invite", payload);
        }

        // public async Task<IEnumerable<FriendRequestDto>> GetSentFriendRequestsAsync()
        // {
        //     var studentId = _identityService.GetCurrentUserId();
        //     string accessToken = await _identityService.GetAccessTokenAsync();
        //     _httpClient.SetAccessToken(accessToken);
        //     return await _httpClient.GetAsync<IEnumerable<FriendRequestDto>>($"friends/requests/sent/{studentId}");
        // }

        public async Task<StudentDto> GetUserDetails(Guid userId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentDto>($"students/{userId}");
        }


        public async Task<IEnumerable<FriendRequestDto>> GetSentFriendRequestsAsync()
        {
            try
            {
                var studentId = _identityService.GetCurrentUserId();
                if (studentId == Guid.Empty)
                {
                    throw new InvalidOperationException("User ID is not valid.");
                }

                string accessToken = await _identityService.GetAccessTokenAsync();
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new InvalidOperationException("Access token is missing or invalid.");
                }

                _httpClient.SetAccessToken(accessToken);
                var friendRequests = await _httpClient.GetAsync<IEnumerable<FriendRequestDto>>($"friends/requests/sent/{studentId}");

                foreach (var request in friendRequests)
                {
                    var userDetails = await GetUserDetails(request.InviteeId);
                    request.InviteeName = userDetails.FirstName + " " + userDetails.LastName;
                    request.InviteeEmail = userDetails.Email;
                    request.InviteeImage = userDetails.ProfileImage;
                }

                return friendRequests;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your error-handling policy
                return new List<FriendRequestDto>();
            }
        }

        public async Task<IEnumerable<FriendRequestDto>> GetIncomingFriendRequestsAsync()
        {
            var userId = _identityService.GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                throw new InvalidOperationException("User ID is not valid.");
            }

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var endpoint = $"friends/requests/{userId}";
            return await _httpClient.GetAsync<IEnumerable<FriendRequestDto>>(endpoint);
        }
    }    
}
