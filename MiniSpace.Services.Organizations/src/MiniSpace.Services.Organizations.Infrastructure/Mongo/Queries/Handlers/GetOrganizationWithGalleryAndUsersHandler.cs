using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationWithGalleryAndUsersHandler : IQueryHandler<GetOrganizationWithGalleryAndUsers, OrganizationGalleryUsersDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;
        private readonly IMongoRepository<OrganizationGalleryImageDocument, Guid> _galleryRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;

        public GetOrganizationWithGalleryAndUsersHandler(
            IMongoRepository<OrganizationDocument, Guid> organizationRepository,
            IMongoRepository<OrganizationGalleryImageDocument, Guid> galleryRepository,
            IOrganizationRolesRepository organizationRolesRepository)
        {
            _organizationRepository = organizationRepository;
            _galleryRepository = galleryRepository;
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task<OrganizationGalleryUsersDto> HandleAsync(GetOrganizationWithGalleryAndUsers query, CancellationToken cancellationToken)
        {
            // Fetch the organization document from the repository
            var organizationDocument = await _organizationRepository.GetAsync(o => o.Id == query.OrganizationId);
            if (organizationDocument == null)
            {
                return null;
            }

            var organization = organizationDocument.AsEntity();

            var galleryDocument = await _galleryRepository.FindAsync(g => g.OrganizationId == organization.Id);
            var gallery = galleryDocument?.SelectMany(doc => doc.Gallery).Select(g => g.AsEntity()) ?? Enumerable.Empty<GalleryImage>();

            var roles = await _organizationRolesRepository.GetRolesAsync(organization.Id);

            var settingsDto = organization.Settings != null 
                ? new OrganizationSettingsDto(organization.Settings) 
                : new OrganizationSettingsDto();

            return new OrganizationGalleryUsersDto(organization, gallery, organization.Users)
            {
                OrganizationDetails = new OrganizationDetailsDto(organization)
                {
                    Settings = settingsDto,
                    Roles = roles?.Select(r => new RoleDto(r)).ToList() ?? new List<RoleDto>()
                }
            };
        }
    }
}
