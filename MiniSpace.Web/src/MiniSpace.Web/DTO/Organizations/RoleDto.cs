using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Enums;

namespace MiniSpace.Web.DTO
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<Permission, bool> Permissions { get; set; }

    }
}
