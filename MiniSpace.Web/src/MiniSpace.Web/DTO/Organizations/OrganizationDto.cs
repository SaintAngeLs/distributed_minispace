using System;

namespace MiniSpace.Web.DTO.Organizations
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RootId { get; set; }
    }    
}
