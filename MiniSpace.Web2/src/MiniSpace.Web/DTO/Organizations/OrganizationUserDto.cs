using System;

namespace MiniSpace.Web.DTO.Organizations
{
    public class OrganizationUserDto
    {
        public Guid Id { get; set; }
        public RoleDto Role { get; set; }
    }
}
