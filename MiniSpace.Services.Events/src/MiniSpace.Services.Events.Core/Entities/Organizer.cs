using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Organizer
    {
        public Guid Id { get; set; } 
        public Guid? UserId { get; set; } 
        public Guid? OrganizationId { get; set; } 
        public OrganizerType OrganizerType { get; set; } 

        public Organizer(Guid id, OrganizerType organizerType, Guid? userId = null, Guid? organizationId = null)
        {
            Id = id;
            OrganizerType = organizerType;

            if (organizerType == OrganizerType.User)
            {
                UserId = userId ?? id;
                OrganizationId = null;
            }
            else if (organizerType == OrganizerType.Organization)
            {
                UserId = null;
                OrganizationId = organizationId ?? id;
            }
        }
    }
}
