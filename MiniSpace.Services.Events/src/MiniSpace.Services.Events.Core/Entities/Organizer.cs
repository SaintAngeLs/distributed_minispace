using System;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class Organizer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        
        public Organizer(Guid id, string name, string email, Guid organizationId, string organizationName)
        {
            Id = id;
            Name = name;
            Email = email;
            OrganizationId = organizationId;
            OrganizationName = organizationName;
        }
    }
}