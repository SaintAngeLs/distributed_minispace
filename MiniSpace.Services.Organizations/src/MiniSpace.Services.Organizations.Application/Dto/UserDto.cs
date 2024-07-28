using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class UserDto
    {
        public Guid Id { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; }

        public UserDto()
        {
            
        }

        public UserDto(User user)
        {
            Id = user.Id;
            Roles = user.Roles.Select(r => new RoleDto(r)).ToList();
        }
    }
}
