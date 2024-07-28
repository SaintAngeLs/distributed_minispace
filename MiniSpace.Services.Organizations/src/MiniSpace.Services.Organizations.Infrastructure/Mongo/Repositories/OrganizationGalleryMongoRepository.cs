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
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;

        public OrganizationGalleryMongoRepository(IMongoRepository<OrganizationDocument, Guid> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<GalleryImage> GetImageAsync(Guid organizationId, Guid imageId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            var imageDocument = organization?.Gallery.FirstOrDefault(i => i.Id == imageId);
            return imageDocument?.AsEntity();
        }

        public async Task<IEnumerable<GalleryImage>> GetGalleryAsync(Guid organizationId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == organizationId);
            return organization?.Gallery.Select(g => g.AsEntity());
        }

        public async Task AddImageAsync(GalleryImage image)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == image.OrganizationId);
            if (organization != null)
            {
                var gallery = organization.Gallery.ToList();
                gallery.Add(image.AsDocument());
                organization.Gallery = gallery;
                await _organizationRepository.UpdateAsync(organization);
            }
        }

        public async Task UpdateImageAsync(GalleryImage image)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Id == image.OrganizationId);
            if (organization != null)
            {
                var gallery = organization.Gallery.ToList();
                var imageDocument = gallery.FirstOrDefault(g => g.Id == image.Id);
                if (imageDocument != null)
                {
                    gallery.Remove(imageDocument);
                    gallery.Add(image.AsDocument());
                    organization.Gallery = gallery;
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
        }

        public async Task DeleteImageAsync(Guid imageId)
        {
            var organization = await _organizationRepository.GetAsync(o => o.Gallery.Any(g => g.Id == imageId));
            if (organization != null)
            {
                var gallery = organization.Gallery.ToList();
                var imageDocument = gallery.FirstOrDefault(g => g.Id == imageId);
                if (imageDocument != null)
                {
                    gallery.Remove(imageDocument);
                    organization.Gallery = gallery;
                    await _organizationRepository.UpdateAsync(organization);
                }
            }
        }
    }
}
