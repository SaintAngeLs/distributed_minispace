using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Application.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedUserOrganizationsHandler : IQueryHandler<GetPaginatedUserOrganizations, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public GetPaginatedUserOrganizationsHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>> HandleAsync(GetPaginatedUserOrganizations query, CancellationToken cancellationToken)
        {
            var organizations = await _organizationRepository.GetOrganizationsByUserAsync(query.UserId);

            var matchedOrganizations = new List<OrganizationDto>();

            foreach (var org in organizations)
            {
                var organizationDto = await ConvertToDtoAsync(org);
                matchedOrganizations.Add(organizationDto);

                if (org.SubOrganizations != null && org.SubOrganizations.Any())
                {
                    foreach (var subOrg in org.SubOrganizations)
                    {
                        var subOrganizationDto = await ConvertToDtoAsync(subOrg);
                        matchedOrganizations.Add(subOrganizationDto);
                    }
                }
            }

            var totalItems = matchedOrganizations.Count;
            var paginatedOrganizations = matchedOrganizations
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>(paginatedOrganizations, query.Page, query.PageSize, totalItems);
        }

        private async Task<OrganizationDto> ConvertToDtoAsync(Organization organization)
        {
            var members = await _organizationMembersRepository.GetMembersAsync(organization.Id);

            return new OrganizationDto
            {
                Id = organization.Id,
                Name = organization.Name,
                Description = organization.Description,
                ImageUrl = organization.ImageUrl,
                BannerUrl = organization.BannerUrl,
                OwnerId = organization.OwnerId,
                DefaultRoleName = organization.DefaultRoleName,
                Address = organization.Address,
                Country = organization.Country,
                City = organization.City,
                Telephone = organization.Telephone,
                Email = organization.Email,
                Users = members?.Select(user => new UserDto(user)).ToList(),
                Settings = organization.Settings != null ? new OrganizationSettingsDto(organization.Settings) : null // Include settings
            };
        }
    }
}
