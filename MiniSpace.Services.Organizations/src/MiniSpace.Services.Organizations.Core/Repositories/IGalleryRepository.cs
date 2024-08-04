using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IGalleryRepository
    {
        Task<GalleryImage> GetImageAsync(Guid organizationId, Guid imageId);
        Task<IEnumerable<GalleryImage>> GetImagesAsync(Guid organizationId);
        Task AddImageAsync(GalleryImage image);
        Task RemoveImageAsync(Guid organizationId, Guid imageId);
    }
}
