using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetPaginatedOrganizations : IQuery<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; } 

        public GetPaginatedOrganizations(int page, int pageSize, string search = null)
        {
            Page = page;
            PageSize = pageSize;
            Search = search;
        }
    }
}
