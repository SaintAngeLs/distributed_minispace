using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Services.Clients;

namespace MiniSpace.Services.Events.Infrastructure.Services.Clients
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

        public Task<StudentEventsDto> GetAsync(Guid id)
            => _httpClient.GetAsync<StudentEventsDto>($"{_url}/students/{id}/events");

        public Task<UserDto> GetStudentByIdAsync(Guid studentId)
            => _httpClient.GetAsync<UserDto>($"{_url}/students/{studentId}");
    }
}
