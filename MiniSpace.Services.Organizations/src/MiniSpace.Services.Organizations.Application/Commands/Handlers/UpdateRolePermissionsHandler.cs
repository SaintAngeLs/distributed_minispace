using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Threading.Tasks;
using MiniSpace.Services.Organizations.Application.Exceptions;
using System.Threading;
using System.Collections.Generic;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class UpdateRolePermissionsHandler : ICommandHandler<UpdateRolePermissions>
    {
        private readonly IOrganizationRolesRepository _rolesRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateRolePermissionsHandler(IOrganizationRolesRepository rolesRepository, IOrganizationRepository organizationRepository)
        {
            _rolesRepository = rolesRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task HandleAsync(UpdateRolePermissions command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            var role = await _rolesRepository.GetRoleByNameAsync(command.OrganizationId, command.RoleName);
            if (role == null)
            {
                throw new RoleNotFoundException(command.RoleName);
            }

            role.UpdateName(command.RoleName);
            role.UpdateDescription(command.Description);

            var permissions = new Dictionary<Permission, bool>();
            foreach (var permission in command.Permissions)
            {
                // Convert the string key to match enum case sensitivity
                if (Enum.TryParse<Permission>(permission.Key, true, out var parsedPermission))
                {
                    permissions[parsedPermission] = permission.Value;
                }
                else
                {
                    throw new InvalidPermissionException(permission.Key);
                }
            }
            role.UpdatePermissions(permissions);

            await _rolesRepository.UpdateRoleAsync(role);
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
