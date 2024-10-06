using Paralax.CQRS.Queries;
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
        private readonly IOrganizationMembersRepository _organizationMembersRepository; 

        public GetOrganizationWithGalleryAndUsersHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationGalleryRepository galleryRepository,
            IOrganizationRolesRepository organizationRolesRepository,
            IOrganizationMembersRepository organizationMembersRepository) 
        {
            _organizationRepository = organizationRepository;
            _galleryRepository = galleryRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _organizationMembersRepository = organizationMembersRepository; 
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
                galleryImages = Enumerable.Empty<GalleryImage>();
            }

            var roles = await _organizationRolesRepository.GetRolesAsync(organization.Id);
            var users = await _organizationMembersRepository.GetMembersAsync(organization.Id);

            var userDtos = users?.Select(u => new UserDto
            {
                Id = u.Id,
                Role = new RoleDto
                {
                    Id = u.Role?.Id ?? Guid.Empty,
                    Name = u.Role?.Name ?? "Unknown",
                    Description = u.Role?.Description ?? string.Empty,
                    Permissions = u.Role?.Permissions ?? new Dictionary<Permission, bool>()
                }
            }).ToList() ?? new List<UserDto>();

            var settingsDto = organization.Settings != null 
                ? new OrganizationSettingsDto(organization.Settings) 
                : new OrganizationSettingsDto();

            var result = new OrganizationGalleryUsersDto(organization, galleryImages, users) 
            {
                OrganizationDetails = new OrganizationDetailsDto(organization)
                {
                    Settings = settingsDto,
                    Roles = roles?.Select(r => new RoleDto(r)).ToList() ?? new List<RoleDto>()
                },
                Gallery = galleryImages.Select(g => new GalleryImageDto(g)).ToList(),
                Users = userDtos 
            };

            // Console.WriteLine("Result Object:");
            // var resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            // Console.WriteLine(resultJson);

            return result;
        }
    }
}
