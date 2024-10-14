using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Services.Clients;

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

        public async Task<bool> StudentExistsAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_url}/students/{id}");
            return response != null;
        }

        public Task<UserFromServiceDto> GetStudentByIdAsync(Guid studentId)
            => _httpClient.GetAsync<UserFromServiceDto>($"{_url}/students/{studentId}");

    }
}