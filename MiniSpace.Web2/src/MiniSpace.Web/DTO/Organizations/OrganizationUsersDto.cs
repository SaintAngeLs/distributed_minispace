using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Organizations
{
    
    public class OrganizationUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
