using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Organizations
{
    
    public class OrganizationUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<AuthUserDto> Users { get; set; }
    }
}
