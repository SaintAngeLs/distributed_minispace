using System;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationCreated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }

        public OrganizationCreated(Guid organizationId, string name, DateTime createdAt)
        {
            OrganizationId = organizationId;
            Name = name;
            CreatedAt = createdAt;
        }
    }
}