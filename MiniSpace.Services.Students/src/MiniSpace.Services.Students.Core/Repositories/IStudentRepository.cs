using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Guid id);
    }
}
