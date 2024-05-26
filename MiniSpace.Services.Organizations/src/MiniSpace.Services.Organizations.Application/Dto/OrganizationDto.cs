using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RootId { get; set; }

        public OrganizationDto()
        {
            
        }
        
        public OrganizationDto (Organization organization, Guid rootId)
        {
            Id = organization.Id;
            Name = organization.Name;
            RootId = rootId;
        }
    }
}

