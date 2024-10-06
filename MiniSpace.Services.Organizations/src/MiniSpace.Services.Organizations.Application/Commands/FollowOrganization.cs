using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class FollowOrganization : ICommand
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }

        public FollowOrganization(Guid userId, Guid organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}
