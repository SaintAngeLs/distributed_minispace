using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserOrganizations : IQuery<IEnumerable<UserOrganizationsDto>>
    {
        public Guid UserId { get; set; }

        public GetUserOrganizations(Guid userId)
        {
            UserId = userId;
        }
    }
}
