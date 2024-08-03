using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrganizationGalleryMongoRepository : IOrganizationGalleryRepository
    {
        private readonly IMongoRepository<OrganizationGalleryImageDocument, Guid> _galleryRepository;

        public OrganizationGalleryMongoRepository(IMongoRepository<OrganizationGalleryImageDocument, Guid> galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }

        public async Task<GalleryImage> GetImageAsync(Guid organizationId, Guid imageId)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            var imageDocument = galleryDocument?.Gallery.FirstOrDefault(i => i.Id == imageId);
            return imageDocument?.AsEntity();
        }

        public async Task<IEnumerable<GalleryImage>> GetGalleryAsync(Guid organizationId)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            return galleryDocument?.Gallery.Select(g => g.AsEntity());
        }

        public async Task AddImageAsync(Guid organizationId, GalleryImage image)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            if (galleryDocument != null)
            {
                var gallery = galleryDocument.Gallery.ToList();
                gallery.Add(image.AsDocument());
                galleryDocument.Gallery = gallery;
                await _galleryRepository.UpdateAsync(galleryDocument);
            }
            else
            {
                galleryDocument = new OrganizationGalleryImageDocument
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organizationId,
                    Gallery = new List<GalleryImageEntry> { image.AsDocument() }
                };
                await _galleryRepository.AddAsync(galleryDocument);
            }
        }

        public async Task UpdateImageAsync(Guid organizationId, GalleryImage image)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            if (galleryDocument != null)
            {
                var gallery = galleryDocument.Gallery.ToList();
                var imageDocument = gallery.FirstOrDefault(g => g.Id == image.Id);
                if (imageDocument != null)
                {
                    gallery.Remove(imageDocument);
                    gallery.Add(image.AsDocument());
                    galleryDocument.Gallery = gallery;
                    await _galleryRepository.UpdateAsync(galleryDocument);
                }
            }
        }

        public async Task DeleteImageAsync(Guid organizationId, Guid imageId)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            if (galleryDocument != null)
            {
                var gallery = galleryDocument.Gallery.ToList();
                var imageDocument = gallery.FirstOrDefault(g => g.Id == imageId);
                if (imageDocument != null)
                {
                    gallery.Remove(imageDocument);
                    galleryDocument.Gallery = gallery;
                    await _galleryRepository.UpdateAsync(galleryDocument);
                }
            }
        }

        public async Task RemoveImageAsync(Guid organizationId, string mediaFileUrl)
        {
            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == organizationId);
            if (galleryDocument != null)
            {
                var gallery = galleryDocument.Gallery.ToList();
                var imageDocument = gallery.FirstOrDefault(g => g.Url == mediaFileUrl);
                if (imageDocument != null)
                {
                    gallery.Remove(imageDocument);
                    galleryDocument.Gallery = gallery;
                    await _galleryRepository.UpdateAsync(galleryDocument);
                }
            }
        }
    }
}
