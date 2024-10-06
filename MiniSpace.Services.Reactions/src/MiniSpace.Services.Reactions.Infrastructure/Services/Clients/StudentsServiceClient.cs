using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Reactions.Application.Services.Clients;

namespace MiniSpace.Services.Reactions.Infrastructure.Services.Clients
{
   [ExcludeFromCodeCoverage]
    public class StudentsServiceClient : IStudentsServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public StudentsServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["students"];
        }

        public async Task<bool> StudentExistsAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_url}/students/{id}");
            return response != null;
        }

    }
}