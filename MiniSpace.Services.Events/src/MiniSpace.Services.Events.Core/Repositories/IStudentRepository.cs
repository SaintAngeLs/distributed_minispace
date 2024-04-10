using System;
using System.Threading.Tasks;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Student student);
    }
}