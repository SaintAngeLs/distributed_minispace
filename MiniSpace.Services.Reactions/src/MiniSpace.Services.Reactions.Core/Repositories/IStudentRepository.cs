using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(Student student);


        
        // Task DeleteAsync(Guid id);
    }
}
