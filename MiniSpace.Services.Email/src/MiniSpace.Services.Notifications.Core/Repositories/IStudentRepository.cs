using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Guid id);
    }
}
