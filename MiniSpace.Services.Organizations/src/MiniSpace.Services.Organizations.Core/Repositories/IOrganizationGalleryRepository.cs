using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Repositories
{
    public interface IOrganizationGalleryRepository
    {
        Task<GalleryImage> GetImageAsync(Guid organizationId, Guid imageId);
        Task<IEnumerable<GalleryImage>> GetGalleryAsync(Guid organizationId);
        Task AddImageAsync(Guid organizationId, GalleryImage image);
        Task UpdateImageAsync(Guid organizationId, GalleryImage image);
        Task DeleteImageAsync(Guid organizationId, Guid imageId);
        Task RemoveImageAsync(Guid organizationId, string mediaFileUrl);
    }
}
