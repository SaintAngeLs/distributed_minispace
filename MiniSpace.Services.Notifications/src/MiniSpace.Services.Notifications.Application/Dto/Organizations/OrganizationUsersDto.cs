using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Dto.Organizations
{
    public class OrganizationUsersDto
    {
        public OrganizationDto Organization { get; set; }
        public IEnumerable<OrganizationUserDto> Users { get; set; }
    }
}