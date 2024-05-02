using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public IEnumerable<Guid> Organizers { get; set; }
    }
}