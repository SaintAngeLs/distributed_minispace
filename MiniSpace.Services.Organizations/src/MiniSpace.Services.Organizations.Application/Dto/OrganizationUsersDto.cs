using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    
    [ExcludeFromCodeCoverage]
    public class OrganizationUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<UserDto> Users { get; set; }

        public OrganizationUsersDto(Organization organization, IEnumerable<User> users)
        {
            Organization = new OrganizationDto(organization);
            Users = users.Select(u => new UserDto(u)).ToList();
        }
    }
}
