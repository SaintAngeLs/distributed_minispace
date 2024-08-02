using System;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationSettingsUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public OrganizationSettings Settings { get; }

        public OrganizationSettingsUpdated(Guid organizationId, OrganizationSettings settings)
        {
            OrganizationId = organizationId;
            Settings = settings;
        }
    }
}