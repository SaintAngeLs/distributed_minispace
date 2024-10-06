using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{


    [ExcludeFromCodeCoverage]
    public class GetOrganizationWithGalleryHandler : IQueryHandler<GetOrganizationWithGallery, OrganizationGalleryDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;
        private readonly IMongoRepository<OrganizationGalleryImageDocument, Guid> _galleryRepository;

        public GetOrganizationWithGalleryHandler(
            IMongoRepository<OrganizationDocument, Guid> organizationRepository,
            IMongoRepository<OrganizationGalleryImageDocument, Guid> galleryRepository)
        {
            _organizationRepository = organizationRepository;
            _galleryRepository = galleryRepository;
        }

        public async Task<OrganizationGalleryDto> HandleAsync(GetOrganizationWithGallery query, CancellationToken cancellationToken)
        {
            var organizationDocument = await _organizationRepository.GetAsync(o => o.Id == query.OrganizationId);
            if (organizationDocument == null)
            {
                return null;
            }

            var galleryDocument = await _galleryRepository.GetAsync(g => g.OrganizationId == query.OrganizationId);
            var gallery = galleryDocument?.Gallery.Select(g => g.AsEntity()) ?? Enumerable.Empty<GalleryImage>();

            var organization = organizationDocument.AsEntity();
            return new OrganizationGalleryDto(organization, gallery);
        }
    }
}
