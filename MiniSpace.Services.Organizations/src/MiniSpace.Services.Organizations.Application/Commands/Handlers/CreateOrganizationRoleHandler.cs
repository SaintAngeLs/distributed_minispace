using Convey.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class CreateOrganizationRoleHandler : ICommandHandler<CreateOrganizationRole>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IAppContext _appContext;

        public CreateOrganizationRoleHandler(IOrganizationRepository organizationRepository, IOrganizationRolesRepository organizationRolesRepository, IAppContext appContext)
        {
            _organizationRepository = organizationRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateOrganizationRole command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            var user = await _organizationRepository.GetMemberAsync(command.OrganizationId, identity.Id);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not a member of the organization.");
            }

            var role = await _organizationRolesRepository.GetRoleByNameAsync(organization.Id, user.Role.Name);

            if (role == null || !(role.Permissions.ContainsKey(Permission.EditPermissions) && role.Permissions[Permission.EditPermissions])
                            && !(role.Permissions.ContainsKey(Permission.AssignRoles) && role.Permissions[Permission.AssignRoles]))
            {
                throw new UnauthorizedAccessException("User does not have permission to create roles.");
            }

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

            var newRole = new Role(command.RoleName, command.Description, permissions);
            organization.AddRole(newRole);

            await _organizationRolesRepository.AddRoleAsync(command.OrganizationId, newRole);
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
