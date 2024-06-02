using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Email.Core.Entities;

namespace MiniSpace.Services.Email.Core.Repositories
{
    public interface IStudentEmailsRepository
    {
        Task<StudentEmails> GetByStudentIdAsync(Guid studentId);
        Task AddAsync(StudentEmails studentEmails);
        Task UpdateAsync(StudentEmails studentEmails);
        Task AddOrUpdateAsync(StudentEmails studentEmails);
        Task DeleteAsync(Guid studentId);
        Task UpdateEmailStatus(Guid studentId, Guid emailNotificationId, EmailNotificationStatus newStatus);
        Task<bool> EmailExists(Guid studentId, Guid emailNotificationId);
        Task DeleteEmail(Guid studentId, Guid emailNotificationId);
    }
}
