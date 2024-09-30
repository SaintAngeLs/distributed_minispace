using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetChildrenOrganizations : IQuery<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        public Guid OrganizationId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetChildrenOrganizations(Guid organizationId, int page, int pageSize)
        {
            OrganizationId = organizationId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
