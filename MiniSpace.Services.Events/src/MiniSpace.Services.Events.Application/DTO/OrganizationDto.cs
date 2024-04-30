using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Organizers;
    }
}