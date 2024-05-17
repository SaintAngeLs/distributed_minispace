using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public OrganizationDto()
        {
            
        }
        
        public OrganizationDto (Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
        }
    }
}

