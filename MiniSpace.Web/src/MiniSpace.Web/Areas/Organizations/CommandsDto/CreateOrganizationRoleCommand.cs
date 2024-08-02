using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Organizations;

namespace MiniSpace.Web.Areas.Organizations.CommandsDto
{
    public class CreateOrganizationRoleCommand
    {
         public Guid OrganizationId { get; }
        public string RoleName { get; }
        public Dictionary<string, bool> Permissions { get; }
    }
}
