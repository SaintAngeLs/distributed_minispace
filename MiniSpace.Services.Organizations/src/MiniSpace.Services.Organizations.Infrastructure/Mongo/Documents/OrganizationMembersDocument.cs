using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationMembersDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<UserEntry> Users { get; set; }
    }

    public class UserEntry
    {
        public Guid UserId { get; set; }
        public RoleAssignment Role { get; set; }
    }

    public class RoleAssignment
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
