using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Student student);
    }
}
