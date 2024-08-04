using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationRoles : IQuery<IEnumerable<RoleDto>>
    {
        public Guid OrganizationId { get; set; }
    }
}
