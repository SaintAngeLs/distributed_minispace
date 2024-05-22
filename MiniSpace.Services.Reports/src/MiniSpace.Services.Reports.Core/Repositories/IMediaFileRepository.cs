using MiniSpace.Services.Reports.Core.Entities;

namespace MiniSpace.Services.Reports.Core.Repositories
{
    public interface IMediaFileRepository
    {
        Task<MediaFile> GetAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(MediaFile mediaFile);
        Task DeleteAsync(Guid id);
    }
}