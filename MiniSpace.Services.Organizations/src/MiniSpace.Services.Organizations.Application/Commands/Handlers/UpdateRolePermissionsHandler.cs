using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Core.Entities;
using System;
using System.Threading.Tasks;
using MiniSpace.Services.Organizations.Application.Exceptions;

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
            // Retrieve the organization
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            // Retrieve the role
            var role = await _rolesRepository.GetRoleAsync(command.OrganizationId, command.RoleId);
            if (role == null)
            {
                throw new RoleNotFoundException(command.RoleId);
            }

            // Update role permissions
            var permissions = new Dictionary<Permission, bool>();
            foreach (var permission in command.Permissions)
            {
                if (Enum.TryParse<Permission>(permission.Key, out var parsedPermission))
                {
                    permissions[parsedPermission] = permission.Value;
                }
                else
                {
                    throw new InvalidPermissionException(permission.Key);
                }
            }
            organization.UpdateRolePermissions(role.Id, permissions);

            // Save changes
            await _rolesRepository.UpdateRoleAsync(role);
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
