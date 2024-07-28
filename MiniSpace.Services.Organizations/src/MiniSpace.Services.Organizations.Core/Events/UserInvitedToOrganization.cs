using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class UserInvitedToOrganization : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public string Email { get; }
        public DateTime InvitedAt { get; }

        public UserInvitedToOrganization(Guid organizationId, Guid userId, string email, DateTime invitedAt)
        {
            OrganizationId = organizationId;
            UserId = userId;
            Email = email;
            InvitedAt = invitedAt;
        }
    }
}