using Convey.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Events
{
    public class OrganizationCreated: IEvent
    {
        public Guid OrganizationId { get; }
        public string Name { get; }
        public Guid ParentId { get; }

        public OrganizationCreated(Guid organizationId, string name, Guid parentId)
        {
            OrganizationId = organizationId;
            Name = name;
            ParentId = parentId;
        }
    }
}