using System;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class InviteUserToOrganizationCommand
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }

        public InviteUserToOrganizationCommand(Guid organizationId, Guid userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }
    }
}
