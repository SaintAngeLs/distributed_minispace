using System.Collections;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Organizers { get; set; }

        public OrganizationDetailsDto()
        {
            
        }

        public OrganizationDetailsDto(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Organizers = organization.Organizers.Select(o => o.Id);
        }
    }
}