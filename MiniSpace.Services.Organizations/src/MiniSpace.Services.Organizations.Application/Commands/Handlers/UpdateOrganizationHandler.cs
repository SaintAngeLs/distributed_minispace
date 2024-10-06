using Paralax.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class UpdateOrganizationHandler : ICommandHandler<UpdateOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public UpdateOrganizationHandler(
            IOrganizationRepository organizationRepository, 
            IOrganizationRolesRepository organizationRolesRepository,
            IAppContext appContext, 
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var existingOrganization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (existingOrganization == null)
            {
                var root = await _organizationRepository.GetAsync(command.RootId);
                if (root == null)
                {
                    throw new RootOrganizationNotFoundException(command.RootId);
                }

                var parent = root.GetSubOrganization(command.ParentId);
                if (parent == null)
                {
                    throw new ParentOrganizationNotFoundException(command.ParentId);
                }

                if (string.IsNullOrWhiteSpace(command.Name))
                {
                    throw new InvalidOrganizationNameException(command.Name);
                }

                var newOrganization = new Organization(
                    command.OrganizationId,
                    command.Name,
                    command.Description,
                    command.Settings,
                    command.OwnerId,
                    command.BannerUrl,
                    command.ImageUrl,
                    command.ParentId,
                    command.DefaultRoleName,
                    command.Address,   // New fields
                    command.Country,
                    command.City,
                    command.Telephone,
                    command.Email,
                    null // Sub-organizations, initialized later if needed
                );

                parent.AddSubOrganization(newOrganization);
                await _organizationRepository.UpdateAsync(root);
            }
            else
            {
                var user = await _organizationRepository.GetMemberAsync(existingOrganization.Id, identity.Id);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("User does not have permission to update the organization.");
                }

                var role = await _organizationRolesRepository.GetRoleByNameAsync(existingOrganization.Id, user.Role.Name);
                if (role == null || !role.Permissions.ContainsKey(Permission.EditOrganizationDetails) || !role.Permissions[Permission.EditOrganizationDetails])
                {
                    throw new UnauthorizedAccessException("User does not have permission to update the organization.");
                }

                existingOrganization.UpdateDetails(
                    command.Name, 
                    command.Description, 
                    command.Settings, 
                    command.BannerUrl, 
                    command.ImageUrl,
                    command.Address,   // New fields
                    command.Country,
                    command.City,
                    command.Telephone,
                    command.Email
                );

                existingOrganization.UpdateDefaultRole(command.DefaultRoleName);
                await _organizationRepository.UpdateAsync(existingOrganization);
            }

            await _messageBroker.PublishAsync(new OrganizationUpserted(
                command.OrganizationId,
                existingOrganization != null,
                DateTime.UtcNow));
        }
    }
}
