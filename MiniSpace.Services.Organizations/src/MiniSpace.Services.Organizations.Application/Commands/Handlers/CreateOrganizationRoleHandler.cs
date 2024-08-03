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

        public CreateOrganizationRoleHandler(IOrganizationRepository organizationRepository, IOrganizationRolesRepository organizationRolesRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task HandleAsync(CreateOrganizationRole command, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
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

            var role = new Role(command.RoleName, "Default role description", permissions);
            organization.AddRole(role);

            // Corrected the method call by passing both organizationId and role
            await _organizationRolesRepository.AddRoleAsync(command.OrganizationId, role);
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
