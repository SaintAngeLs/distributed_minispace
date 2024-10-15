using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.Areas.Friends.CommandsDto;
using Astravent.Web.Wasm.Areas.PagedResult;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.DTO.Friends;

namespace Astravent.Web.Wasm.Areas.Friends
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
                return;
            }

            await _httpClient.DeleteAsync($"friends/{requesterId}/{friendId}/remove");
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

        public async Task<PaginatedResponseDto<StudentDto>> GetAllStudentsAsync(int page = 1, int pageSize = 10, string searchTerm = null)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"students?page={page}&pageSize={pageSize}&searchTerm={searchTerm}";
            return await _httpClient.GetAsync<PaginatedResponseDto<StudentDto>>(url);
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

        public async Task<IEnumerable<FriendRequestDto>> GetSentFriendRequestsAsync()
        {
            var studentId = _identityService.GetCurrentUserId();
            if (studentId == Guid.Empty) return Enumerable.Empty<FriendRequestDto>();

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/sent/{studentId}";
            var userRequests = await _httpClient.GetAsync<IEnumerable<UserRequestsDto>>(url);

            if (userRequests == null || !userRequests.Any())
            {
                return Enumerable.Empty<FriendRequestDto>();
            }

            var sentRequests = userRequests.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in sentRequests)
            {
                var userDetails = await GetStudentAsync(request.InviteeId);
                request.InviteeName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviteeEmail = userDetails.Email;
            }

            return sentRequests;
        }

        public async Task<IEnumerable<FriendRequestDto>> GetIncomingFriendRequestsAsync()
        {
            var userId = _identityService.GetCurrentUserId();
            if (userId == Guid.Empty) return Enumerable.Empty<FriendRequestDto>();

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/incoming/{userId}";
            var userRequests = await _httpClient.GetAsync<IEnumerable<UserRequestsDto>>(url);

            if (userRequests == null || !userRequests.Any())
            {
                return Enumerable.Empty<FriendRequestDto>();
            }

            var incomingRequests = userRequests.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in incomingRequests)
            {
                var userDetails = await GetStudentAsync(request.InviterId);
                request.InviterName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviterEmail = userDetails.Email;
            }

            return incomingRequests;
        }

        public async Task<PagedResult<FriendRequestDto>> GetSentFriendRequestsPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var userId = _identityService.GetCurrentUserId();
            if (userId == Guid.Empty) return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/sent/{userId}/paginated?page={page}&pageSize={pageSize}";
            var sentRequestsResponse = await _httpClient.GetAsync<PagedResult<UserRequestsDto>>(url);

            if (sentRequestsResponse == null || !sentRequestsResponse.Items.Any())
            {
                return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);
            }

            var sentRequests = sentRequestsResponse.Items.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in sentRequests)
            {
                var userDetails = await GetStudentAsync(request.InviteeId);
                request.InviteeName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviteeEmail = userDetails.Email;
            }

            return new PagedResult<FriendRequestDto>(sentRequests, page, pageSize, sentRequestsResponse.TotalItems);
        }

        public async Task<PagedResult<FriendRequestDto>> GetIncomingFriendRequestsPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var userId = _identityService.GetCurrentUserId();
            if (userId == Guid.Empty) return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);

            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);

            string url = $"friends/requests/{userId}/paginated?page={page}&pageSize={pageSize}";
            var incomingRequestsResponse = await _httpClient.GetAsync<PagedResult<UserRequestsDto>>(url);

            if (incomingRequestsResponse == null || !incomingRequestsResponse.Items.Any())
            {
                return new PagedResult<FriendRequestDto>(Enumerable.Empty<FriendRequestDto>(), page, pageSize, 0);
            }

            var incomingRequests = incomingRequestsResponse.Items.SelectMany(request => request.FriendRequests).ToList();

            foreach (var request in incomingRequests)
            {
                var userDetails = await GetStudentAsync(request.InviterId);
                request.InviterName = $"{userDetails.FirstName} {userDetails.LastName}";
                request.InviterEmail = userDetails.Email;
            }

            return new PagedResult<FriendRequestDto>(incomingRequests, page, pageSize, incomingRequestsResponse.TotalItems);
        }

        public async Task<PagedResult<FriendDto>> GetPagedFollowersAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            
            string url = $"friends/{userId}/followers?page={page}&pageSize={pageSize}";
            var userFollowers = await _httpClient.GetAsync<PagedResult<UserFriendsDto>>(url);

            var allFollowers = userFollowers.Items.SelectMany(uf => uf.Friends).ToList();

            foreach (var follower in allFollowers)
            {
                follower.StudentDetails = await GetStudentAsync(follower.UserId);
            }

            return new PagedResult<FriendDto>(allFollowers, userFollowers.Page, userFollowers.PageSize, userFollowers.TotalItems);
        }

        public async Task<PagedResult<FriendDto>> GetPagedFollowingAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            
            string url = $"friends/{userId}/following?page={page}&pageSize={pageSize}";
            var userFollowing = await _httpClient.GetAsync<PagedResult<UserFriendsDto>>(url);

            var allFollowing = userFollowing.Items.SelectMany(uf => uf.Friends).ToList();

            foreach (var following in allFollowing)
            {
                following.StudentDetails = await GetStudentAsync(following.FriendId);
            }

            return new PagedResult<FriendDto>(allFollowing, userFollowing.Page, userFollowing.PageSize, userFollowing.TotalItems);
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
