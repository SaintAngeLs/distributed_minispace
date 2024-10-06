using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Enums;

namespace Astravent.Web.Wasm.DTO.Organizations
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<Permission, bool> Permissions { get; set; }

    }
}
