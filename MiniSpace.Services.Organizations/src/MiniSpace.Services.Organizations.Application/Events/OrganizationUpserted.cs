using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationUpserted : IEvent
    {
        public Guid OrganizationId { get; }
        public bool WasUpdated { get; }
        public DateTime Timestamp { get; }

        public OrganizationUpserted(Guid organizationId, bool wasUpdated, DateTime timestamp)
        {
            OrganizationId = organizationId;
            WasUpdated = wasUpdated;
            Timestamp = timestamp;
        }
    }
}
