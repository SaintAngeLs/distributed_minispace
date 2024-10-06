using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class UserInvitedToOrganization : IEvent
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
