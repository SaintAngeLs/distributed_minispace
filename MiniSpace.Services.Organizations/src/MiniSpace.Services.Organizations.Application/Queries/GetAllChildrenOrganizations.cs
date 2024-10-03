﻿using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAllChildrenOrganizations: IQuery<IEnumerable<Guid>>
    {
        public Guid OrganizationId { get; set; }
        public Guid RootId { get; set; }
    }
}