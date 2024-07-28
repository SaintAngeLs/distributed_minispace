using Convey.CQRS.Events;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class RootOrganizationCreated : IEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }

        public RootOrganizationCreated(Guid organizationId, string name, DateTime createdAt)
        {
            OrganizationId = organizationId;
            Name = name;
            CreatedAt = createdAt;
        }
    }
}
