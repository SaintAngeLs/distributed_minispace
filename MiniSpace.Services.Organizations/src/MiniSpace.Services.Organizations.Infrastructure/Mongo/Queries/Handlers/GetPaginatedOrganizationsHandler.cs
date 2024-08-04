using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Queries.Handlers
{
    public class GetPaginatedOrganizationsHandler : IQueryHandler<GetPaginatedOrganizations, DTO.PagedResult<OrganizationDto>>
{
    private readonly IOrganizationReadOnlyRepository _repository;

    public GetPaginatedOrganizationsHandler(IOrganizationReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<DTO.PagedResult<OrganizationDto>> HandleAsync(GetPaginatedOrganizations query, CancellationToken cancellationToken)
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
                    matchedOrganizations.Add(new OrganizationDto
                    {
                        Id = org.Id,
                        Name = org.Name,
                        Description = org.Description,
                        ImageUrl = org.ImageUrl,
                    });
                }

                // Check the sub-organizations
                if (org.SubOrganizations != null && org.SubOrganizations.Any())
                {
                    foreach (var subOrg in org.SubOrganizations)
                    {
                        if (subOrg.Name.ToLower().Contains(searchFilter))
                        {
                            matchedOrganizations.Add(new OrganizationDto
                            {
                                Id = subOrg.Id,
                                Name = subOrg.Name,
                                Description = subOrg.Description,
                                ImageUrl = subOrg.ImageUrl,
                            });
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
                matchedOrganizations.Add(new OrganizationDto
                {
                    Id = org.Id,
                    Name = org.Name,
                    Description = org.Description,
                    ImageUrl = org.ImageUrl,
                });

                // Add all sub-organizations
                if (org.SubOrganizations != null && org.SubOrganizations.Any())
                {
                    foreach (var subOrg in org.SubOrganizations)
                    {
                        matchedOrganizations.Add(new OrganizationDto
                        {
                            Id = subOrg.Id,
                            Name = subOrg.Name,
                            Description = subOrg.Description,
                            ImageUrl = subOrg.ImageUrl,
                        });
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
        return new DTO.PagedResult<OrganizationDto>(paginatedOrganizations, query.Page, query.PageSize, totalItems);
    }
}


}


    

