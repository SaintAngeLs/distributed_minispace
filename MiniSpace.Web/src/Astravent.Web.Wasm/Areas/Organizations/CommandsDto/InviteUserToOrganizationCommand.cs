using System;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
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
