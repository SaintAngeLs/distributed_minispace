using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateOrganizationRole : ICommand
    {
        public Guid OrganizationId { get; }
        public string RoleName { get; }
        public Dictionary<string, bool> Permissions { get; }

        public CreateOrganizationRole(Guid organizationId, string roleName, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleName = roleName;
            Permissions = permissions;
        }
    }
}
