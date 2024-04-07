using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;

namespace MiniSpace.Web.Areas.Students
{
    public interface IStudentsService
    {
        Task<StudentDto> GetStudentAsync(Guid studentId);
        Task UpdateStudentAsync(Guid studentId, string profileImage, string description, bool emailNotifications);
        Task CompleteStudentRegistrationAsync(Guid studentId, string firstName, string lastName,
            string profileImage, string description, DateTime dateOfBirth, bool emailNotifications);
    }
}
