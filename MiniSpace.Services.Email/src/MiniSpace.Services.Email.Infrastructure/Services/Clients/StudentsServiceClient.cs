using System;
using System.Threading.Tasks;
using Paralax.HTTP;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Application.Services.Clients;

namespace MiniSpace.Services.Email.Infrastructure.Services.Clients
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
        
        public Task<StudentDto> GetAsync(Guid id)
            => _httpClient.GetAsync<StudentDto>($"{_url}/students/{id}");

        public Task<IEnumerable<StudentDto>> GetAllAsync()
            => _httpClient.GetAsync<IEnumerable<StudentDto>>($"{_url}/students");

    }
}