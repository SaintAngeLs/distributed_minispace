using System;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizerDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public OrganizerType OrganizerType { get; set; }

        public OrganizerDto()
        {
        }

        public OrganizerDto(Organizer organizer)
        {
            OrganizerType = organizer.OrganizerType;

            if (organizer.OrganizerType == OrganizerType.User)
            {
                Id = organizer.Id;
                UserId = organizer.UserId;
                OrganizationId = null;
            }
            else if (organizer.OrganizerType == OrganizerType.Organization)
            {
                Id = organizer.OrganizationId ?? Guid.Empty;
                UserId = organizer.UserId;
                OrganizationId = organizer.OrganizationId;
            }
        }
    }
}
