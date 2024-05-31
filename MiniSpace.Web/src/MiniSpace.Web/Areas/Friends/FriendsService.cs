using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<FriendDto>> GetAllFriendsAsync(Guid studentId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var url = $"friends/{studentId}";
            var studentFriends = await _httpClient.GetAsync<IEnumerable<StudentFriendsDto>>(url);

            var allFriends = studentFriends.SelectMany(sf => sf.Friends).ToList();

            foreach (var friend in allFriends)
            {
                friend.StudentDetails = await GetStudentAsync(friend.FriendId);
            }

            return allFriends;
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
            var requesterId = _identityService.GetCurrentUserId();
            // Console.WriteLine($"Requester ID: {requesterId}"); // Log the requester ID

            if (requesterId == Guid.Empty)
            {
                // Console.WriteLine("Invalid Requester ID: ID is empty.");
                return; // Optionally handle the case where the requester ID is invalid
            }

            var payload = new { RequesterId = requesterId, FriendId = friendId };
            // Console.WriteLine($"Payload: {payload.RequesterId}, {payload.FriendId}");
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

        public async Task<PaginatedResponseDto<StudentDto>> GetAllStudentsAsync(int page = 1, int resultsPerPage = 10,
            string searchTerm = null)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            string url = $"students?page={page}&resultsPerPage={resultsPerPage}&name={searchTerm}";
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
                var studentRequests = await _httpClient.GetAsync<IEnumerable<StudentRequestsDto>>($"friends/requests/sent/{studentId}");

                if (studentRequests == null || !studentRequests.Any())
                {
                    return Enumerable.Empty<FriendRequestDto>();
                }

                var friendRequests = studentRequests.SelectMany(request => request.FriendRequests).ToList();

                var inviteeIds = friendRequests.Select(r => r.InviteeId).Distinct();
                var userDetailsTasks = inviteeIds.Select(id => GetUserDetails(id));
                var userDetailsResults = await Task.WhenAll(userDetailsTasks);

                var userDetailsDict = userDetailsResults.ToDictionary(user => user.Id, user => user);

                foreach (var request in friendRequests)
                {
                    if (userDetailsDict.TryGetValue(request.InviteeId, out var userDetails))
                    {
                        request.InviteeName = $"{userDetails.FirstName} {userDetails.LastName}";
                        request.InviteeEmail = userDetails.Email;
                        // request.InviteeImage = userDetails.ProfileImage; // Uncomment if you have a profile image field
                    }
                }

                return friendRequests;
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // Console.WriteLine($"Error retrieving sent friend requests: {ex.Message}");
                return new List<FriendRequestDto>();
            }
        }

        public async Task<IEnumerable<FriendRequestDto>> GetIncomingFriendRequestsAsync()
        {
            try
            {
                var userId = _identityService.GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    throw new InvalidOperationException("User ID is not valid.");
                }

                string accessToken = await _identityService.GetAccessTokenAsync();
                _httpClient.SetAccessToken(accessToken);

                var studentRequests = await _httpClient.GetAsync<IEnumerable<StudentRequestsDto>>($"friends/requests/{userId}");
                if (studentRequests == null || !studentRequests.Any())
                {
                    return Enumerable.Empty<FriendRequestDto>();
                }

                var incomingRequests = studentRequests.SelectMany(request => request.FriendRequests).ToList();

                var inviterIds = incomingRequests.Select(r => r.InviterId).Distinct();
                var userDetailsTasks = inviterIds.Select(id => GetUserDetails(id));
                var userDetailsResults = await Task.WhenAll(userDetailsTasks);

                var userDetailsDict = userDetailsResults.ToDictionary(user => user.Id, user => user);

                foreach (var request in incomingRequests)
                {
                    if (userDetailsDict.TryGetValue(request.InviterId, out var userDetails))
                    {
                        request.InviterName = $"{userDetails.FirstName} {userDetails.LastName}";
                        request.InviterEmail = userDetails.Email;
                    }
                }

                return incomingRequests;
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Error retrieving incoming friend requests: {ex.Message}");
                return new List<FriendRequestDto>();
            }
        }


        // private async Task<StudentDto> GetUserDetails(Guid userId)
        // {
        //     string accessToken = await _identityService.GetAccessTokenAsync();
        //     _httpClient.SetAccessToken(accessToken);
        //     return await _httpClient.GetAsync<StudentDto>($"students/{userId}");
        // }

        public async Task AcceptFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var payload = new { RequesterId = requesterId, FriendId = friendId };
            await _httpClient.PostAsync<object, object>($"friends/requests/{requestId}/accept", payload);
        }

        public async Task DeclineFriendRequestAsync(Guid requestId, Guid requesterId, Guid friendId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var payload = new { RequesterId = requesterId, FriendId = friendId };
            await _httpClient.PostAsync<object, object>($"friends/requests/{requestId}/decline", payload);
        }

        public async Task WithdrawFriendRequestAsync(Guid inviterId, Guid inviteeId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var payload = new { InviterId = inviterId, InviteeId = inviteeId };
            await _httpClient.PutAsync<object, object>($"friends/requests/{inviteeId}/withdraw", payload);
        }
    }    
}
