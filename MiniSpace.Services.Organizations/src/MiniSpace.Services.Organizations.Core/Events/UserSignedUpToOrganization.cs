using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class UserSignedUpToOrganization : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime SignedUpAt { get; }

        public UserSignedUpToOrganization(Guid organizationId, Guid userId, DateTime signedUpAt)
        {
            OrganizationId = organizationId;
            UserId = userId;
            SignedUpAt = signedUpAt;
        }
    }
} 