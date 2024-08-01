using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class UpdateRolePermissionsCommand
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public Dictionary<string, bool> Permissions { get; }

        public UpdateRolePermissionsCommand(Guid organizationId, Guid roleId, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            Permissions = permissions;
        }
    }
}
