using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizerAddedToOrganization: IEvent
    {
        public Guid OrganizationId { get; }
        public Guid OrganizerId { get; }

        public OrganizerAddedToOrganization(Guid organizationId, Guid organizerId)
        {
            OrganizationId = organizationId;
            OrganizerId = organizerId;
        }
    }
}