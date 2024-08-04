using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizationWithGalleryAndUsersHandler : IQueryHandler<GetOrganizationWithGalleryAndUsers, OrganizationGalleryUsersDto>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationGalleryRepository _galleryRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;

        public GetOrganizationWithGalleryAndUsersHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationGalleryRepository galleryRepository,
            IOrganizationRolesRepository organizationRolesRepository)
        {
            _organizationRepository = organizationRepository;
            _galleryRepository = galleryRepository;
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task<OrganizationGalleryUsersDto> HandleAsync(GetOrganizationWithGalleryAndUsers query, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(query.OrganizationId);
            if (organization == null)
            {
                return null;
            }

            var galleryImages = await _galleryRepository.GetGalleryAsync(organization.Id);

            if (galleryImages == null)
            {
                Console.WriteLine("Gallery Images Retrieved: null");
                galleryImages = Enumerable.Empty<GalleryImage>();
            }
            else
            {
                Console.WriteLine("Gallery Images Retrieved:");
                Console.WriteLine(JsonSerializer.Serialize(galleryImages, new JsonSerializerOptions { WriteIndented = true }));
            }

            var roles = await _organizationRolesRepository.GetRolesAsync(organization.Id);

            var settingsDto = organization.Settings != null 
                ? new OrganizationSettingsDto(organization.Settings) 
                : new OrganizationSettingsDto();

            var result = new OrganizationGalleryUsersDto(organization, galleryImages, organization.Users)
            {
                OrganizationDetails = new OrganizationDetailsDto(organization)
                {
                    Settings = settingsDto,
                    Roles = roles?.Select(r => new RoleDto(r)).ToList() ?? new List<RoleDto>()
                },
                Gallery = galleryImages.Select(g => new GalleryImageDto(g)).ToList(),
                Users = organization.Users?.Select(u => new UserDto(u)).ToList() ?? new List<UserDto>()
            };

            return result;
        }
    }
}
