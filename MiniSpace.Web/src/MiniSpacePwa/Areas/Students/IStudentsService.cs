using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpacePwa.DTO;
using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Students
{
    public interface IStudentsService
    {
        StudentDto StudentDto { get; }
        Task UpdateStudentDto(Guid studentId);
        void ClearStudentDto();
        Task<StudentDto> GetStudentAsync(Guid studentId);
        Task<PaginatedResponseDto<StudentDto>> GetStudentsAsync();
        Task UpdateStudentAsync(Guid studentId, Guid profileImage, string description, bool emailNotifications);
        Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, Guid profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications);
        Task<string> GetStudentStateAsync(Guid studentId);
    }
}
