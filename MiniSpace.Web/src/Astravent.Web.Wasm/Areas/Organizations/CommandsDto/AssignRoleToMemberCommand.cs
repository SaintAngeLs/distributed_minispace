using System;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class AssignRoleToMemberCommand
    {
        public Guid OrganizationId { get; }
        public Guid MemberId { get; }
        public string Role { get; }

        public AssignRoleToMemberCommand(Guid organizationId, Guid memberId, string role)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
            Role = role;
        }
    }
}
