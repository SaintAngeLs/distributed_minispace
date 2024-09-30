using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetUserFollowOrganizations : IQuery<IEnumerable<OrganizationGalleryUsersDto>>
    {
        public Guid UserId { get; set; }

        public GetUserFollowOrganizations(Guid userId)
        {
            UserId = userId;
        }
    }
}
