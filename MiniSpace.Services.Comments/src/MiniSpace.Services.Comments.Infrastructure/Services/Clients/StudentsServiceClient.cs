using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Services.Clients;

namespace MiniSpace.Services.Comments.Infrastructure.Services.Clients
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

         public Task<UserDto> GetAsync(Guid id)
            => _httpClient.GetAsync<UserDto>($"{_url}/students/{id}");

    }
}