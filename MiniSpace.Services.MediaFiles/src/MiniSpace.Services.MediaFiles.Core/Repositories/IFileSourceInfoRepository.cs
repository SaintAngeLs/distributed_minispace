using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Repositories
{
    public interface IFileSourceInfoRepository
    {
        Task<FileSourceInfo> GetAsync(Guid id);
        Task<IEnumerable<FileSourceInfo>> GetAllUnassociatedAsync();
        Task AddAsync(FileSourceInfo fileSourceInfo);
        Task UpdateAsync(FileSourceInfo fileSourceInfo);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<FileSourceInfo>> FindAsync(Guid sourceId, ContextType sourceType);
    }    
}