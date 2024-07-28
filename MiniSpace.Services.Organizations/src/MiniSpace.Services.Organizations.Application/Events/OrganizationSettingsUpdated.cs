using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Core.Entities;
using System;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationSettingsUpdated : IEvent
    {
        public Guid OrganizationId { get; }
        public OrganizationSettings Settings { get; }
        public DateTime UpdatedAt { get; }

        public OrganizationSettingsUpdated(Guid organizationId, OrganizationSettings settings, DateTime updatedAt)
        {
            OrganizationId = organizationId;
            Settings = settings;
            UpdatedAt = updatedAt;
        }
    }
}
