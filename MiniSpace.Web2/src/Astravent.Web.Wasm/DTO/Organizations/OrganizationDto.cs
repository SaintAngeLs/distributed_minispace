using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

namespace Astravent.Web.Wasm.DTO.Organizations
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? RootId { get; set; } 
        public string DefaultRoleName { get; set; } 
        
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public IEnumerable<OrganizationUserDto> Users { get; set; } = new List<OrganizationUserDto>();
        public OrganizationSettingsDto Settings { get; set; } = new OrganizationSettingsDto();
        public int UserCount => Users?.Count() ?? 0;  

        public bool IsExpanded { get; set; } = false;
        public List<OrganizationDto> SubOrganizations { get; set; } = new List<OrganizationDto>();
    }    
}
