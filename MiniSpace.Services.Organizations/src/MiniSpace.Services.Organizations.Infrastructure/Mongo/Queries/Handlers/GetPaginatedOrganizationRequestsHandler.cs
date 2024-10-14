using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;

namespace MiniSpace.Services.Organizations.Infrastructure.Queries.Handlers
{
    public class GetPaginatedOrganizationRequestsHandler : IQueryHandler<GetPaginatedOrganizationRequests, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationRequestDto>>
    {
        private readonly IOrganizationRequestsRepository _requestsRepository;

        public GetPaginatedOrganizationRequestsHandler(IOrganizationRequestsRepository requestsRepository)
        {
            _requestsRepository = requestsRepository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationRequestDto>> HandleAsync(GetPaginatedOrganizationRequests query, CancellationToken cancellationToken)
        {
            // Retrieve all requests for the organization
            var requests = await _requestsRepository.GetRequestsAsync(query.OrganizationId);
            
            if (!requests.Any())
            {
                return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationRequestDto>(Enumerable.Empty<OrganizationRequestDto>(), query.Page, query.PageSize, 0);
            }

            // Count total items
            var totalItems = requests.Count();

            // Paginate the requests
            var paginatedRequests = requests
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(r => new OrganizationRequestDto
                {
                    RequestId = r.Id,
                    UserId = r.UserId,
                    RequestDate = r.RequestDate,
                    State = r.State.ToString(),
                    Reason = r.Reason
                })
                .ToList();

            // Return a paged result
            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationRequestDto>(paginatedRequests, query.Page, query.PageSize, totalItems);
        }
    }
}
