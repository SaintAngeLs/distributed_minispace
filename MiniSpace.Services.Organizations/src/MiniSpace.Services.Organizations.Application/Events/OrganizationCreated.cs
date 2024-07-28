using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationCreated : IEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public Guid ParentId { get; }
        public DateTime CreatedAt { get; }

        public OrganizationCreated(Guid organizationId, string name, Guid parentId, DateTime createdAt)
        {
            OrganizationId = organizationId;
            Name = name;
            ParentId = parentId;
            CreatedAt = createdAt;
        }
    }
}
