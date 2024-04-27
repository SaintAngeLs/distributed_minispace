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

        [Fact]
        public async Task InviteFriend_Endpoint_Returns_AlreadyFriends_Error()
        {
            string accessToken = await AuthenticateAndGetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new StringContent(JsonSerializer.Serialize(new
            {
                inviterId = "fd0176f4-736c-49cd-b57a-523d544ae3d3",
                inviteeId = "b202a5fe-afbd-4894-95e3-41aff99f430c"
            }), Encoding.UTF8, "application/json");

            HttpResponseMessage inviteResponse = await _client.PostAsync("http://localhost:5012/friends/b202a5fe-afbd-4894-95e3-41aff99f430c/invite", payload);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, inviteResponse.StatusCode);

            var responseContent = await inviteResponse.Content.ReadAsStringAsync();
            Assert.Contains("Already friends", responseContent);
        }

    }
}
