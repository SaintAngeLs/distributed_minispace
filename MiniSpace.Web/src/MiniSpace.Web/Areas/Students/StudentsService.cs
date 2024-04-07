using System;
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

        public StudentsService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }
        
        public Task<UserDto> GetStudentAsync(Guid studentId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return _httpClient.GetAsync<UserDto>($"students/{studentId}");
        }

        public Task UpdateStudentAsync(Guid studentId, string profileImage, string description, bool emailNotifications)
            => _httpClient.PutAsync($"students/{studentId}", new {studentId, profileImage,
                description, emailNotifications});

        public Task CompleteStudentRegistrationAsync(Guid studentId, string firstName, string lastName,
            string profileImage, string description, DateTime dateOfBirth, bool emailNotifications)
            => _httpClient.PostAsync("students", new {studentId, firstName, lastName, profileImage,
                description, dateOfBirth, emailNotifications});
    }    
}
