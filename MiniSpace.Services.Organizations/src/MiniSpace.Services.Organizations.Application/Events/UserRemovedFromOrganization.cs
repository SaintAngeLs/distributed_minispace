using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class UserRemovedFromOrganization : IEvent
    {
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime Timestamp { get; }

        public UserRemovedFromOrganization(Guid organizationId, Guid userId, DateTime timestamp)
        {
            OrganizationId = organizationId;
            UserId = userId;
            Timestamp = timestamp;
        }
    }
}
