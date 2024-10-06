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
    public class CreateSubOrganizationHandler : ICommandHandler<CreateSubOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateSubOrganizationHandler(
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

        public async Task HandleAsync(CreateSubOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

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

            var user = await _organizationRepository.GetMemberAsync(root.Id, identity.Id);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not a member of the organization.");
            }

            var role = await _organizationRolesRepository.GetRoleByNameAsync(root.Id, user.Role.Name);

            if (role == null || !(role.Permissions.ContainsKey(Permission.CreateSubGroups) && role.Permissions[Permission.CreateSubGroups]))
            {
                throw new UnauthorizedAccessException("User does not have permission to create sub-organizations.");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new InvalidOrganizationNameException(command.Name);
            }

            var organization = new Organization(
                command.SubOrganizationId, 
                command.Name, 
                command.Description, 
                command.Settings, 
                command.OwnerId, 
                command.BannerUrl, 
                command.ImageUrl);
            
            parent.AddSubOrganization(organization);
            await _organizationRepository.UpdateAsync(root);
            await _messageBroker.PublishAsync(new OrganizationCreated(
                organization.Id, 
                organization.Name, 
                organization.Description, 
                command.RootId, 
                command.ParentId, 
                command.OwnerId, 
                DateTime.UtcNow));
        }
    }
}
