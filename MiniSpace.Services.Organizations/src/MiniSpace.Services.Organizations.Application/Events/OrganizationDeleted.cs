using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationDeleted:IEvent
    {
        public Guid OrganizationId { get; }

        public OrganizationDeleted(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
    }
}