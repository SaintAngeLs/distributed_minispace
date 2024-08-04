using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

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
            // Fetch the organization entity from the repository
            var organization = await _organizationRepository.GetAsync(query.OrganizationId);
            if (organization == null)
            {
                return null;
            }

            // Fetch gallery images associated with the organization
            var galleryImages = await _galleryRepository.GetGalleryAsync(organization.Id);
            var gallery = galleryImages?.Select(g => new GalleryImageDto(g)).ToList() 
                          ?? new List<GalleryImageDto>();

            // Log gallery images
            Console.WriteLine("Gallery Images Retrieved:");
            Console.WriteLine(JsonSerializer.Serialize(galleryImages, new JsonSerializerOptions { WriteIndented = true }));

            // Fetch roles associated with the organization
            var roles = await _organizationRolesRepository.GetRolesAsync(organization.Id);

            // Convert organization settings to DTO
            var settingsDto = organization.Settings != null 
                ? new OrganizationSettingsDto(organization.Settings) 
                : new OrganizationSettingsDto();

            // Create the final DTO to return
            var result = new OrganizationGalleryUsersDto(organization, galleryImages, organization.Users)
            {
                OrganizationDetails = new OrganizationDetailsDto(organization)
                {
                    Settings = settingsDto,
                    Roles = roles?.Select(r => new RoleDto(r)).ToList() ?? new List<RoleDto>()
                },
                Gallery = gallery,  // Set the gallery images here
                Users = organization.Users?.Select(u => new UserDto(u)).ToList() ?? new List<UserDto>()
            };

            // Log the result
            Console.WriteLine("Final Result Returned:");
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));

            return result;
        }
    }
}
