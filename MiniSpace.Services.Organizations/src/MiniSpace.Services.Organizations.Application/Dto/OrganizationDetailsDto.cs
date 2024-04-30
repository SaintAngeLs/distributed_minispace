using System.Collections;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public IEnumerable<OrganizerDto> Organizers { get; set; }
    }
}