using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationInvitationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<InvitationEntry> Invitations { get; set; }
    }

    public class InvitationEntry
    {
        public Guid UserId { get; set; }
    }
}
