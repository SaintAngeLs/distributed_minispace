using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<RoleAssignment> Roles { get; set; }
    }

    public class RoleAssignment
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
