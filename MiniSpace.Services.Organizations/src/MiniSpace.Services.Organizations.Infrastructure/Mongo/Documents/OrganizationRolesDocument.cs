using Convey.Types;
using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationRolesDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<RoleEntry> Roles { get; set; }
    }

    public class RoleEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<Permission, bool> Permissions { get; set; }
    }
}
