using System;

namespace MiniSpace.Web.DTO
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public bool IsLeaf { get; set; }
    }    
}
