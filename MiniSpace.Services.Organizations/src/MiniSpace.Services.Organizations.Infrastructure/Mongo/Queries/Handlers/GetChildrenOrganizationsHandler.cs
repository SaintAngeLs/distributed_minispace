using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetChildrenOrganizationsHandler : IQueryHandler<GetChildrenOrganizations, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetChildrenOrganizationsHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>> HandleAsync(GetChildrenOrganizations query, CancellationToken cancellationToken)
        {
            // Fetch the root organization
            var rootOrganization = await _organizationRepository.GetAsync(query.OrganizationId);
            if (rootOrganization == null)
            {
                return MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>.Empty(query.Page, query.PageSize);
            }

            // Recursively fetch all sub-organizations
            var childOrganizations = GetChildOrganizations(rootOrganization);

            // Convert to DTOs
            var childOrganizationDtos = childOrganizations.Select(o => new OrganizationDto(o)).ToList();

            // Apply pagination
            var totalItems = childOrganizationDtos.Count;
            var paginatedOrganizations = childOrganizationDtos
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>(paginatedOrganizations, query.Page, query.PageSize, totalItems);
        }

        private IEnumerable<Organization> GetChildOrganizations(Organization parent)
        {
            var organizations = new List<Organization>();

            if (parent.SubOrganizations != null && parent.SubOrganizations.Any())
            {
                organizations.AddRange(parent.SubOrganizations);

                foreach (var subOrganization in parent.SubOrganizations)
                {
                    organizations.AddRange(GetChildOrganizations(subOrganization));
                }
            }

            return organizations;
        }
    }
}
