using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetPaginatedOrganizationRequests : IQuery<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationRequestDto>>
    {
        public Guid OrganizationId { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetPaginatedOrganizationRequests(Guid organizationId, int page, int pageSize)
        {
            OrganizationId = organizationId;
            Page = page;
            PageSize = pageSize;
        }
    }
}