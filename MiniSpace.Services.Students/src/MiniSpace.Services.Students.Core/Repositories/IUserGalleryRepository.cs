using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserGalleryRepository
    {
        Task<UserGallery> GetAsync(Guid userId);
        Task AddAsync(UserGallery userGallery);
        Task UpdateAsync(UserGallery userGallery);
        Task DeleteAsync(Guid userId);
        Task<IEnumerable<GalleryImage>> GetGalleryImagesAsync(Guid userId);
        Task AddGalleryImageAsync(Guid userId, GalleryImage galleryImage);
        Task RemoveGalleryImageAsync(Guid userId, Guid imageId);
    }
}
