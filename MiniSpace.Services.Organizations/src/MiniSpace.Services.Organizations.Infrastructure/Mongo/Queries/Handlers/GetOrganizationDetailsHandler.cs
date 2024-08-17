using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizationDetailsHandler : IQueryHandler<GetOrganizationDetails, OrganizationDetailsDto>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationGalleryRepository _galleryRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public GetOrganizationDetailsHandler(
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

        public async Task<OrganizationDetailsDto> HandleAsync(GetOrganizationDetails query, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(query.OrganizationId);
            if (organization == null)
            {
                return null;
            }

            var galleryImages = await _galleryRepository.GetGalleryAsync(organization.Id);
            var roles = await _organizationRolesRepository.GetRolesAsync(organization.Id);
            var users = await _organizationMembersRepository.GetMembersAsync(organization.Id);

            var userDtos = users?.Select(u => new UserDto(u)).ToList() ?? new List<UserDto>();

            var organizationDto = new OrganizationDetailsDto(organization)
            {
                SubOrganizations = organization.SubOrganizations?.Select(subOrg => new SubOrganizationDto(subOrg)).ToList(),
                Invitations = organization.Invitations?.Select(invite => new InvitationDto(invite)).ToList(),
                Users = userDtos,
                Roles = roles?.Select(role => new RoleDto(role)).ToList() ?? new List<RoleDto>(),
                Gallery = galleryImages?.Select(galleryImage => new GalleryImageDto(galleryImage)).ToList() ?? new List<GalleryImageDto>(),
                Settings = organization.Settings != null ? new OrganizationSettingsDto(organization.Settings) : null
            };

            return organizationDto;
        }
    }
}
