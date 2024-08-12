using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class UserLeftOrganization : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid OrganizationId { get; }
        public DateTime LeftDate { get; }

        public UserLeftOrganization(Guid userId, Guid organizationId, DateTime leftDate)
        {
            UserId = userId;
            OrganizationId = organizationId;
            LeftDate = leftDate;
        }
    }
}
