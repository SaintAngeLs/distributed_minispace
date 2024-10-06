using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Queries.Handlers
{
    public class GetPaginatedOrganizationsHandler : IQueryHandler<GetPaginatedOrganizations, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        private readonly IOrganizationReadOnlyRepository _repository;
        private readonly IOrganizationMembersRepository _membersRepository;

        public GetPaginatedOrganizationsHandler(IOrganizationReadOnlyRepository repository, IOrganizationMembersRepository membersRepository)
        {
            _repository = repository;
            _membersRepository = membersRepository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>> HandleAsync(GetPaginatedOrganizations query, CancellationToken cancellationToken)
        {
            // Fetch all organizations
            var organizationsQuery = _repository.GetAll();

            // Prepare the list to hold the results
            var matchedOrganizations = new List<OrganizationDto>();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(query.Search))
            {
                var searchFilter = query.Search.ToLower();

                foreach (var org in await organizationsQuery.ToListAsync(cancellationToken))
                {
                    // Check the main organization
                    if (org.Name.ToLower().Contains(searchFilter))
                    {
                        var organizationDto = await ConvertToDtoAsync(org);
                        matchedOrganizations.Add(organizationDto);
                    }

                    // Check the sub-organizations
                    if (org.SubOrganizations != null && org.SubOrganizations.Any())
                    {
                        foreach (var subOrg in org.SubOrganizations)
                        {
                            if (subOrg.Name.ToLower().Contains(searchFilter))
                            {
                                var subOrganizationDto = await ConvertToDtoAsync(subOrg);
                                matchedOrganizations.Add(subOrganizationDto);
                            }
                        }
                    }
                }
            }
            else
            {
                // No search filter, just flatten all organizations and sub-organizations
                foreach (var org in await organizationsQuery.ToListAsync(cancellationToken))
                {
                    // Add the main organization
                    var organizationDto = await ConvertToDtoAsync(org);
                    matchedOrganizations.Add(organizationDto);

                    // Add all sub-organizations
                    if (org.SubOrganizations != null && org.SubOrganizations.Any())
                    {
                        foreach (var subOrg in org.SubOrganizations)
                        {
                            var subOrganizationDto = await ConvertToDtoAsync(subOrg);
                            matchedOrganizations.Add(subOrganizationDto);
                        }
                    }
                }
            }

            // Count total items
            var totalItems = matchedOrganizations.Count;
            Console.WriteLine($"Total items found: {totalItems}");

            // Paginate the matched organizations
            var paginatedOrganizations = matchedOrganizations
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            Console.WriteLine($"Items after pagination: {paginatedOrganizations.Count}");

            // Return a paged result
            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>(paginatedOrganizations, query.Page, query.PageSize, totalItems);
        }

        private async Task<OrganizationDto> ConvertToDtoAsync(OrganizationDocument organization)
        {
            // Retrieve members of the organization
            var members = await _membersRepository.GetMembersAsync(organization.Id);

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
                Settings = organization.Settings != null ? new OrganizationSettingsDto(organization.Settings) : null 
            };
        }
    }
}
