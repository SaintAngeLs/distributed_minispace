using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetUserCreatedOrganizations : IQuery<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        public Guid UserId { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetUserCreatedOrganizations(Guid userId, int page, int pageSize)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
