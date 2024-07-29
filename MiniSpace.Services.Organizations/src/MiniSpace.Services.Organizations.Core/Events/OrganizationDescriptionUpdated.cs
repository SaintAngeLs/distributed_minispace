using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationDescriptionUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public string NewDescription { get; }

        public OrganizationDescriptionUpdated(Guid organizationId, string newDescription)
        {
            OrganizationId = organizationId;
            NewDescription = newDescription;
        }
    }
}
