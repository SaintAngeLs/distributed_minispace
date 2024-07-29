using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationNameUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public string NewName { get; }

        public OrganizationNameUpdated(Guid organizationId, string newName)
        {
            OrganizationId = organizationId;
            NewName = newName;
        }
    }
}
