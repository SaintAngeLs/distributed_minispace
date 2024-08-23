using System;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Reactions.Application.Services.Clients;

namespace MiniSpace.Services.Reactions.Infrastructure.Services.Clients
{
    public class CommentServiceClient : ICommentServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public CommentServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["comments"]; 
        }

        public async Task<bool> CommentExistsAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_url}/comments/{id}");
            return response != null;
        }
    }
}
