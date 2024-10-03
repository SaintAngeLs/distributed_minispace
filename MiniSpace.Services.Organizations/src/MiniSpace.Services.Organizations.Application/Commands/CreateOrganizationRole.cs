using Paralax.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class CreateOrganizationRole : ICommand
    {
        public Guid OrganizationId { get; }
        public string RoleName { get; }
        public string Description { get; }
        public Dictionary<string, bool> Permissions { get; }

        public CreateOrganizationRole(Guid organizationId, string roleName, string description, Dictionary<string, bool> permissions)
        {
            OrganizationId = organizationId;
            RoleName = roleName;
            Description = description;
            Permissions = permissions;
        }
    }
}
