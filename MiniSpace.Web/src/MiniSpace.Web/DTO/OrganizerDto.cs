using System;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; } 
        public Guid? UserId { get; set; } 
        public Guid? OrganizationId { get; set; } 
        public OrganizerType OrganizerType { get; set; } 

    }
}
