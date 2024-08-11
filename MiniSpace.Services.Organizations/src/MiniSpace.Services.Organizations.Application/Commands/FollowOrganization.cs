using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class FollowOrganization : ICommand
    {
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public FollowOrganization(Guid userId, Guid organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}
