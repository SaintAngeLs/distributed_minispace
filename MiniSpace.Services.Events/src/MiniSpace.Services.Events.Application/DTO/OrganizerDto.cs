using System;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        
        public OrganizerDto()
        {
        }
        
        public OrganizerDto(Organizer organizer)
        {
            Id = organizer.Id;
            Name = organizer.Name;
            Email = organizer.Email;
            OrganizationId = organizer.OrganizationId;
            OrganizationName = organizer.OrganizationName;
        }
    }
}