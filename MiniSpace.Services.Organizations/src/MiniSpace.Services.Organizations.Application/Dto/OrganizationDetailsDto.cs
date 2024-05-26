using System.Collections;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RootId { get; set; }
        public IEnumerable<Guid> Organizers { get; set; }

        public OrganizationDetailsDto()
        {
            
        }

        public OrganizationDetailsDto(Organization organization, Guid rootId)
        {
            Id = organization.Id;
            Name = organization.Name;
            RootId = rootId;
            Organizers = organization.Organizers.Select(o => o.Id);
        }
    }
}