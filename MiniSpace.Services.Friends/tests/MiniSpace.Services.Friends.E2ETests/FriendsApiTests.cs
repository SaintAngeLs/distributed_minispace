using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MiniSpace.Services.Friends.E2ETests.Auth;

namespace MiniSpace.Services.Friends.E2ETests
{
    public class FriendsApiTests
    {
        private readonly HttpClient _client;
        private readonly string _baseUri = "http://localhost:5012/friends";

        public FriendsApiTests()
        {
            _client = new HttpClient();
        }

        private async Task<string> AuthenticateAndGetToken()
        {
            var loginPayload = new StringContent(JsonSerializer.Serialize(new
            {
                email = "friend1@email.com",
                password = "friend1"
            }), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("http://localhost:5004/sign-in", loginPayload);

            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Authentication failed with status code {response.StatusCode} and content {errorContent}");
                throw new HttpRequestException("Authentication failed, cannot proceed with tests.");
            }


            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);

            return tokenResponse.accessToken;
        }

        private async Task<string> SendFriendRequestAndGetId()
        {
            string accessToken = await AuthenticateAndGetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new StringContent(JsonSerializer.Serialize(new
            {
                inviterId = "fd0176f4-736c-49cd-b57a-523d544ae3d3",
                inviteeId = "b202a5fe-afbd-4894-95e3-41aff99f430c"
            }), Encoding.UTF8, "application/json");

            HttpResponseMessage inviteResponse = await _client.PostAsync($"{_baseUri}/b202a5fe-afbd-4894-95e3-41aff99f430c/invite", payload);

            if (!inviteResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to send friend request.");
            }

            var responseContent = await inviteResponse.Content.ReadAsStringAsync();
            var friendRequestResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
            return friendRequestResponse.friendRequestId;
        }

         [Fact]
        public async Task InviteFriend_Twice_First_Succeeds_Second_Fails()
        {
            string accessToken = await AuthenticateAndGetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new StringContent(JsonSerializer.Serialize(new
            {
                inviterId = "fd0176f4-736c-49cd-b57a-523d544ae3d3",
                inviteeId = "b202a5fe-afbd-4894-95e3-41aff99f430c"
            }), Encoding.UTF8, "application/json");

            // First invitation should succeed with 201 Created
            HttpResponseMessage firstInviteResponse = await _client.PostAsync($"{_baseUri}/b202a5fe-afbd-4894-95e3-41aff99f430c/invite", payload);
            Assert.Equal(System.Net.HttpStatusCode.Created, firstInviteResponse.StatusCode);

            // Second invitation should fail with 400 Bad Request due to already invited status
            HttpResponseMessage secondInviteResponse = await _client.PostAsync($"{_baseUri}/b202a5fe-afbd-4894-95e3-41aff99f430c/invite", payload);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, secondInviteResponse.StatusCode);

            var responseContent = await secondInviteResponse.Content.ReadAsStringAsync();
            Assert.Contains("already_invited", responseContent);
            Assert.Contains("A pending friend request already exists between", responseContent);
        }

        [Fact]
        public async Task AcceptFriendRequest_Endpoint_Succeeds()
        {
            string accessToken = await AuthenticateAndGetToken();
            string friendRequestId = await SendFriendRequestAndGetId();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Assuming that there is an endpoint to create a pending friend request which should be tested as well
            // Simulate accepting a friend request
            HttpResponseMessage response = await _client.GetAsync($"http://localhost:5012/friends/requests/{friendRequestId}/accept");

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Request accepted", responseContent);
        }

        // [Fact]
        // public async Task DeclineFriendRequest_Endpoint_Succeeds()
        // {
        //     string accessToken = await AuthenticateAndGetToken();
        //     string friendRequestId = await SendFriendRequestAndGetId();
        //     _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        //     // Simulate declining a friend request
        //     HttpResponseMessage response = await _client.GetAsync($"http://localhost:5012/friends/requests/{friendRequestId}/decline");

        //     Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        //     var responseContent = await response.Content.ReadAsStringAsync();
        //     Assert.Contains("Request declined", responseContent);
        // }


    }
}
