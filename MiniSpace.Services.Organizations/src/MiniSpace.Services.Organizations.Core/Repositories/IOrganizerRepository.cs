using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizerRepository
    {
        Task<Organizer> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Organizer student);
        Task DeleteAsync(Guid id);
    }
}