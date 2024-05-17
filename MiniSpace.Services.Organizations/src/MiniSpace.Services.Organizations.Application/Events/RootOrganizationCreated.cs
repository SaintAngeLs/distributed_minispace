using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class RootOrganizationCreated: IEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }

        public RootOrganizationCreated(Guid organizationId, string name)
        {
            OrganizationId = organizationId;
            Name = name;
        }
    }
}