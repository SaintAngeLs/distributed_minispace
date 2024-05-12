using System;

namespace MiniSpace.Web.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}