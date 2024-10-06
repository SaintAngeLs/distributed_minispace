using System;
using Astravent.Web.Wasm.DTO.Enums;

namespace Astravent.Web.Wasm.DTO
{
    public class OrganizerDto
    {
        public Guid Id { get; set; } 
        public Guid? UserId { get; set; } 
        public Guid? OrganizationId { get; set; } 
        public OrganizerType OrganizerType { get; set; } 

    }
}
