using System;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Application.DTO
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