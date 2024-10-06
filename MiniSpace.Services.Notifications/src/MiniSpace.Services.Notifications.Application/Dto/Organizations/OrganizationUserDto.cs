using System;

using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Notifications.Application.Dto.Organizations
{
    [ExcludeFromCodeCoverage]
    public class OrganizationUserDto
    {
        public Guid Id { get; set; }
        public OrganizationRoleDto Role { get; set; }

        public OrganizationUserDto()
        {
        }
    }
}
