using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Organizations;

namespace Astravent.Web.Wasm.Areas.Organizations.CommandsDto
{
    public class UpdateRolePermissionsCommand
    {
        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
        public string RoleName { get; set; } 
        public string Description { get; set; } 
        public Dictionary<string, bool> Permissions { get; }

        public UpdateRolePermissionsCommand(Guid organizationId, Guid roleId, string roleName,
            string description, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            Permissions = permissions;
        }
    }
}
