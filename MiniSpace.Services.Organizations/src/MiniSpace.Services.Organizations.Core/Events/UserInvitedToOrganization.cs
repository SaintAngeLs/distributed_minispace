using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class UserInvitedToOrganization : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime InvitedAt { get; }

        public UserInvitedToOrganization(Guid organizationId, Guid userId, DateTime invitedAt)
        {
            OrganizationId = organizationId;
            UserId = userId;
            InvitedAt = invitedAt;
        }
    }
}