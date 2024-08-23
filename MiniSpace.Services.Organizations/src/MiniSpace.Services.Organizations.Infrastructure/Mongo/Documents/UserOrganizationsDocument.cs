using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserOrganizationsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<UserOrganizationEntryDocument> Organizations { get; set; }
    }

    public class UserOrganizationEntryDocument
    {
        public Guid OrganizationId { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
