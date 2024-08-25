using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.Areas.Friends.CommandsDto;
using MiniSpace.Web.Areas.PagedResult;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;
using MiniSpace.Web.DTO.Friends;

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

        public async Task<PagedResult<FriendDto>> GetAllFriendsAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            
            string url = $"friends/{userId}?page={page}&pageSize={pageSize}";
            var userFriends = await _httpClient.GetAsync<PagedResult<UserFriendsDto>>(url);

            var allFriends = userFriends.Items.SelectMany(uf => uf.Friends).ToList();

            foreach (var friend in allFriends)
            {
                friend.StudentDetails = await GetStudentAsync(friend.FriendId);
            }

            return new PagedResult<FriendDto>(allFriends, userFriends.Page, userFriends.PageSize, userFriends.TotalItems);
        }

        public async Task<HttpResponse<object>> AddFriendAsync(Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var payload = new AddFriendRequestDto { FriendId = friendId };
            return await _httpClient.PostAsync<object, object>("friends", payload);
        }

        public async Task RemoveFriendAsync(Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var requesterId = _identityService.GetCurrentUserId();

            if (requesterId == Guid.Empty)
            {
                return; // Optionally handle the case where the requester ID is invalid
            }

            await _httpClient.DeleteAsync($"friends/{requesterId}/{friendId}/remove");
        }

        public async Task<PagedResult<StudentDto>> GetAllStudentsAsync(int page = 1, int pageSize = 10, string searchTerm = null)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"students?page={page}&pageSize={pageSize}&searchTerm={searchTerm}";
            return await _httpClient.GetAsync<PagedResult<StudentDto>>(url);
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

        public async Task<PagedResult<FriendRequestDto>> GetSentFriendRequestsAsync(int page = 1, int pageSize = 10)
        {
            var studentId = _identityService.GetCurrentUserId();
            if (studentId == Guid.Empty) return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/sent/{studentId}?page={page}&pageSize={pageSize}";
            var studentRequests = await _httpClient.GetAsync<PagedResult<UserRequestsDto>>(url);

            if (studentRequests == null || !studentRequests.Items.Any())
            {
                return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);
            }

            var friendRequests = studentRequests.Items.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in friendRequests)
            {
                var userDetails = await GetStudentAsync(request.InviteeId);
                request.InviteeName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviteeEmail = userDetails.Email;
            }

            return new PagedResult<FriendRequestDto>(friendRequests, studentRequests.Page, studentRequests.PageSize, studentRequests.TotalItems);
        }

        public async Task<PagedResult<FriendRequestDto>> GetIncomingFriendRequestsAsync(int page = 1, int pageSize = 10)
        {
            var userId = _identityService.GetCurrentUserId();
            if (userId == Guid.Empty) return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/{userId}?page={page}&pageSize={pageSize}";
            var studentRequests = await _httpClient.GetAsync<PagedResult<UserRequestsDto>>(url);

            if (studentRequests == null || !studentRequests.Items.Any())
            {
                return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);
            }

            var incomingRequests = studentRequests.Items.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in incomingRequests)
            {
                var userDetails = await GetStudentAsync(request.InviterId);
                request.InviterName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviterEmail = userDetails.Email;
            }

            return new PagedResult<FriendRequestDto>(incomingRequests, studentRequests.Page, studentRequests.PageSize, studentRequests.TotalItems);
        }

        public async Task AcceptFriendRequestAsync(FriendRequestActionDto requestAction)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync<object, object>($"friends/requests/{requestAction.RequestId}/accept", requestAction);
        }

        public async Task DeclineFriendRequestAsync(FriendRequestActionDto requestAction)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PostAsync<object, object>($"friends/requests/{requestAction.RequestId}/decline", requestAction);
        }

        public async Task WithdrawFriendRequestAsync(WithdrawFriendRequestDto withdrawRequest)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync<object, object>($"friends/requests/{withdrawRequest.InviteeId}/withdraw", withdrawRequest);
        }
    }
}
