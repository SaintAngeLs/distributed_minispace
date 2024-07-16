using System;
using System.Collections.Generic;

namespace MiniSpacePwa.DTO
{
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RootId { get; set; }
        public IEnumerable<Guid> Organizers { get; set; }
    }
}