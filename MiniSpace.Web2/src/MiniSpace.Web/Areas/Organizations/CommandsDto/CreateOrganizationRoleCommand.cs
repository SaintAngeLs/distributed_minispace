using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class CreateOrganizationRoleCommand
    {
        public Guid OrganizationId { get; }
        public string RoleName { get; }
        public string Description { get; } 
        public Dictionary<string, bool> Permissions { get; }

        public CreateOrganizationRoleCommand(Guid organizationId, string roleName, string description, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleName = roleName;
            Description = description;
            Permissions = permissions;
        }
    }
}
