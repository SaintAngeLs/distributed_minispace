using Paralax.CQRS.Commands;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class CreateOrganizationHandler : ICommandHandler<CreateOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;
        private readonly IOrganizationGalleryRepository _organizationGalleryRepository;
        private readonly IOrganizationRolesRepository _organizationRolesRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IOrganizationGalleryRepository organizationGalleryRepository,
            IOrganizationRolesRepository organizationRolesRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _organizationGalleryRepository = organizationGalleryRepository;
            _organizationRolesRepository = organizationRolesRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateOrganization command, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            Organization organization;

            if (command.ParentId == null)
            {
                organization = new Organization(
                    command.OrganizationId,
                    command.Name,
                    command.Description,
                    command.Settings,
                    command.OwnerId,
                    command.BannerUrl,
                    command.ImageUrl,
                    null, // No parent organization
                    "User", // Set default role to "User"
                    command.Address,   // New fields
                    command.Country,
                    command.City,
                    command.Telephone,
                    command.Email
                );

                await _organizationRepository.AddAsync(organization);
            }
            else
            {
                var root = await _organizationRepository.GetAsync(command.RootId.Value);
                if (root == null)
                {
                    throw new RootOrganizationNotFoundException(command.RootId.Value);
                }

                var parent = root.GetSubOrganization(command.ParentId.Value);
                if (parent == null)
                {
                    throw new ParentOrganizationNotFoundException(command.ParentId.Value);
                }

                organization = new Organization(
                    command.OrganizationId,
                    command.Name,
                    command.Description,
                    command.Settings,
                    command.OwnerId,
                    command.BannerUrl,
                    command.ImageUrl,
                    command.ParentId.Value,
                    "User", // Set default role to "User"
                    command.Address,   // New fields
                    command.Country,
                    command.City,
                    command.Telephone,
                    command.Email
                );

                parent.AddSubOrganization(organization);
                await _organizationRepository.UpdateAsync(root);
            }
            
            var defaultRoles = organization.Roles.ToList();
            foreach (var role in defaultRoles)
            {
                await _organizationRolesRepository.AddRoleAsync(organization.Id, role);
            }

            // We do not have to initialize gallery in the momentum organization is created
            // await _organizationGalleryRepository.AddImageAsync(organization.Id, new GalleryImage(Guid.NewGuid(), "Default Image URL", DateTime.UtcNow));

            // Add the creator as a member with the "Creator" role
            var creatorRole = defaultRoles.SingleOrDefault(r => r.Name == "Creator");
            if (creatorRole == null)
            {
                throw new RoleNotFoundException("Creator");
            }

            var creatorMember = new User(identity.Id, creatorRole);
            await _organizationMembersRepository.AddMemberAsync(organization.Id, creatorMember);

            // The default role is already set to "User" during the organization creation.

            await _messageBroker.PublishAsync(new OrganizationCreated(
                organization.Id,
                organization.Name,
                organization.Description,
                command.RootId ?? organization.Id,
                command.ParentId,
                command.OwnerId,
                DateTime.UtcNow));
        }
    }
}
