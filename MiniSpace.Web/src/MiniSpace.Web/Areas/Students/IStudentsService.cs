using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Students
{
    public interface IStudentsService
    {
        StudentDto StudentDto { get; }
        Task UpdateStudentDto(Guid studentId);
        void ClearStudentDto();
        Task<StudentDto> GetStudentAsync(Guid studentId);
        Task<PaginatedResponseDto<StudentDto>> GetStudentsAsync();
        Task UpdateStudentAsync(Guid studentId, string profileImageUrl, string description, bool emailNotifications, string contactEmail);
        public Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImageUrl,
            string description, DateTime dateOfBirth, bool emailNotifications, string contactEmail);
        Task<string> GetStudentStateAsync(Guid studentId);
    }
}
