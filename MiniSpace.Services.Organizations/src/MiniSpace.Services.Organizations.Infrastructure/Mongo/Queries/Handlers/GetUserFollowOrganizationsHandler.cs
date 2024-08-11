using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserFollowOrganizationsHandler : IQueryHandler<GetUserFollowOrganizations, IEnumerable<OrganizationGalleryUsersDto>>
    {
        private readonly IUserOrganizationsRepository _userOrganizationsRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationGalleryRepository _galleryRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;

        public GetUserFollowOrganizationsHandler(
            IUserOrganizationsRepository userOrganizationsRepository,
            IOrganizationRepository organizationRepository,
            IOrganizationGalleryRepository galleryRepository,
            IOrganizationRolesRepository organizationRolesRepository)
        {
            _userOrganizationsRepository = userOrganizationsRepository;
            _organizationRepository = organizationRepository;
            _galleryRepository = galleryRepository;
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task<IEnumerable<OrganizationGalleryUsersDto>> HandleAsync(GetUserFollowOrganizations query, CancellationToken cancellationToken)
        {
            var organizationIds = await _userOrganizationsRepository.GetUserOrganizationsAsync(query.UserId);
            var organizationDetailsList = new List<OrganizationGalleryUsersDto>();

            foreach (var organizationId in organizationIds)
            {
                var organization = await _organizationRepository.GetAsync(organizationId);
                if (organization == null)
                {
                    continue;
                }

                var galleryImages = await _galleryRepository.GetGalleryAsync(organizationId);
                var roles = await _organizationRolesRepository.GetRolesAsync(organizationId);

                var organizationDetailsDto = new OrganizationDetailsDto(organization)
                {
                    Settings = organization.Settings != null ? new OrganizationSettingsDto(organization.Settings) : new OrganizationSettingsDto(),
                    Roles = roles?.Select(r => new RoleDto(r)).ToList() ?? new List<RoleDto>()
                };

                var organizationGalleryUsersDto = new OrganizationGalleryUsersDto(organization, galleryImages, organization.Users)
                {
                    OrganizationDetails = organizationDetailsDto,
                    Gallery = galleryImages.Select(g => new GalleryImageDto(g)).ToList(),
                    Users = organization.Users?.Select(u => new UserDto(u)).ToList() ?? new List<UserDto>()
                };

                organizationDetailsList.Add(organizationGalleryUsersDto);
            }

            return organizationDetailsList;
        }
    }
}
