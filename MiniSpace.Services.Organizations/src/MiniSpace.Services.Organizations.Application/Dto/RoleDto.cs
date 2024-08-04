using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<Permission, bool> Permissions { get; set; }

        public RoleDto()
        {
            
        }

        public RoleDto(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
            Permissions = role.Permissions;
        }
    }
}
