using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetUserOrganizations : IQuery<IEnumerable<OrganizationDto>>
    {
        public Guid UserId { get; set; }
    }
}
