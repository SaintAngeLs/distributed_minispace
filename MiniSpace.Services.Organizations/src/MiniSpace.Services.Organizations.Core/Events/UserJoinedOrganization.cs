using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class UserJoinedOrganization : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid OrganizationId { get; }
        public DateTime JoinedDate { get; }

        public UserJoinedOrganization(Guid userId, Guid organizationId, DateTime joinedDate)
        {
            UserId = userId;
            OrganizationId = organizationId;
            JoinedDate = joinedDate;
        }
    }
}
