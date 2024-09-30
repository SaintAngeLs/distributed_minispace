using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationCreated : IEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public string Description { get; }
        public Guid? RootId { get; }  
        public Guid? ParentId { get; }  
        public Guid OwnerId { get; }
        public DateTime CreatedAt { get; }

        public OrganizationCreated(Guid organizationId, string name, string description, Guid? rootId, Guid? parentId, Guid ownerId, DateTime createdAt)
        {
            OrganizationId = organizationId;
            Name = name;
            Description = description;
            RootId = rootId;
            ParentId = parentId;
            OwnerId = ownerId;
            CreatedAt = createdAt;
        }
    }
}
