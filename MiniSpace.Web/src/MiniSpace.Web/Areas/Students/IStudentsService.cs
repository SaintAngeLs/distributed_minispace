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
        Task<StudentDto> GetStudentAsync(Guid studentId, Guid secondId);
        Task UpdateStudentAsync(Guid studentId, string profileImage, string description, bool emailNotifications);
        Task<HttpResponse<object>> CompleteStudentRegistrationAsync(Guid studentId, string profileImage,
            string description, DateTime dateOfBirth, bool emailNotifications);
    }
}
