using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver; // Assuming you're using MongoDB driver

namespace MiniSpace.Services.Organizations.Application.Queries.Handlers
{
    public class GetPaginatedOrganizationsHandler : IQueryHandler<GetPaginatedOrganizations, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        private readonly IOrganizationRepository _repository;

        public GetPaginatedOrganizationsHandler(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>> HandleAsync(GetPaginatedOrganizations query, CancellationToken cancellationToken)
        {
            // Fetch all organizations and apply search filter
            var organizationsQuery = _repository.GetAll();

            if (!string.IsNullOrEmpty(query.Search))
            {
                organizationsQuery = organizationsQuery.Where(o => o.Name.Contains(query.Search));
            }

            // Count total items before pagination
            var totalItems = organizationsQuery.Count();

            // Apply pagination
            var items = organizationsQuery
                            .Skip((query.Page - 1) * query.PageSize)
                            .Take(query.PageSize)
                            .Select(o => new OrganizationDto
                            {
                                Id = o.Id,
                                Name = o.Name,
                                Description = o.Description,
                                ImageUrl = o.ImageUrl
                                // Map other properties as needed
                            })
                            .ToList();

            // Return a paged result
            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>(items, query.Page, query.PageSize, totalItems);
        }
    }
}
