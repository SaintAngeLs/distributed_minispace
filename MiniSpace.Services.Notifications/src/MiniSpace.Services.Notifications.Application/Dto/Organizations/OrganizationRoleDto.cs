using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Notifications.Application.Dto.Organizations
{
    [ExcludeFromCodeCoverage]
    public class OrganizationRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, bool> Permissions { get; set; }
    }
}
