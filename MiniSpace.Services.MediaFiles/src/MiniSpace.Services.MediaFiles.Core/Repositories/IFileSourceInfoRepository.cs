using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Repositories
{
    public interface IFileSourceInfoRepository
    {
        Task AddAsync(FileSourceInfo fileSourceInfo);
        Task DeleteAsync(string url);
        Task<bool> ExistsAsync(string url);
        Task<IEnumerable<FileSourceInfo>> FindAsync(Guid sourceId, ContextType sourceType);
        Task<IEnumerable<FileSourceInfo>> GetAllUnassociatedAsync();
        Task<FileSourceInfo> GetAsync(string url);
        Task UpdateAsync(FileSourceInfo fileSourceInfo);
    }    
}