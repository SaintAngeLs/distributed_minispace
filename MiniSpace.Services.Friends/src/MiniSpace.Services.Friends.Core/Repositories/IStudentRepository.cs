using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Student student);
        Task DeleteAsync(Guid id);
    }
}