using System;
using System.Text.Json;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Application.Services.Clients;

namespace MiniSpace.Services.Communication.Infrastructure.Services.Clients
{
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