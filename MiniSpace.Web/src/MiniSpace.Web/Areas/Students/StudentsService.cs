using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Students
{
    public class StudentsService : IStudentsService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public StudentDto StudentDto { get; private set; }
        
        public StudentsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task UpdateStudentDto(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            StudentDto = await _httpClient.GetAsync<StudentDto>($"students/{studentId}");
        }

        public void ClearStudentDto()
        {
            StudentDto = null;
        }
        
        public async Task<StudentDto> GetStudentAsync(Guid studentId)
        {
            var accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<StudentDto>($"students/{studentId}");
        }

        public Task<IEnumerable<StudentDto>> GetStudentsAsync()
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<IEnumerable<StudentDto>>("students");
        }

        public Task UpdateStudentAsync(Guid studentId, string profileImage, string description, bool emailNotifications)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.PutAsync($"students/{studentId}", new {studentId, profileImage,
                description, emailNotifications});
        }

        public Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications)
            => _httpClient.PostAsync<object,object>("students", new {studentId, profileImage,
                description, dateOfBirth, emailNotifications});

        public async Task<string> GetStudentStateAsync(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);
            return student != null ? student.State : "invalid"; 
        }
    }    
}
