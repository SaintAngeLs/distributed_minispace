using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetPaginatedUserOrganizations : IQuery<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetPaginatedUserOrganizations(Guid userId, int page, int pageSize)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
