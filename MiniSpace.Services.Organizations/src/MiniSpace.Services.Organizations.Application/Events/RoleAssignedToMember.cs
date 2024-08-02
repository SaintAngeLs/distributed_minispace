using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class RoleAssignedToMember : IEvent
    {
        public Guid OrganizationId { get; }
        public Guid MemberId { get; }
        public string Role { get; }
        public DateTime AssignedAt { get; }

        public RoleAssignedToMember(Guid organizationId, Guid memberId, string role, DateTime assignedAt)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
            Role = role;
            AssignedAt = assignedAt;
        }
    }
}
