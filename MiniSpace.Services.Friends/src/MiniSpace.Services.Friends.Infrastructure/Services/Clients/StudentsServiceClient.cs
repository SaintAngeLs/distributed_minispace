using System;
using System.Threading.Tasks;
using Convey.HTTP;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure.Services.Clients
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

        public Task<IEnumerable<UserDto>> GetAllAsync()
            => _httpClient.GetAsync<IEnumerable<UserDto>>($"{_url}/students");

    }
}