using Convey.CQRS.Commands;
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
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IOrganizationGalleryRepository organizationGalleryRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _organizationGalleryRepository = organizationGalleryRepository;
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
                // Create as a root organization
                organization = new Organization(
                    command.OrganizationId,
                    command.Name,
                    command.Description,
                    command.Settings,
                    command.OwnerId,
                    command.BannerUrl,
                    command.ImageUrl,
                    null // No parent organization
                );

                await _organizationRepository.AddAsync(organization);

                // Initialize an empty gallery for the organization
                await _organizationGalleryRepository.AddImageAsync(organization.Id, new GalleryImage(Guid.NewGuid(), "Default Image URL", DateTime.UtcNow));

                // Add the creator as a member with the "Creator" role
                var creatorRole = organization.Roles.SingleOrDefault(r => r.Name == "Creator");
                if (creatorRole == null)
                {
                    throw new RoleNotFoundException("Creator");
                }

                var creatorMember = new User(identity.Id, creatorRole);
                await _organizationMembersRepository.AddMemberAsync(organization.Id, creatorMember);

                await _messageBroker.PublishAsync(new OrganizationCreated(
                    organization.Id,
                    organization.Name,
                    organization.Description,
                    organization.Id, // Root ID is the organization's own ID
                    null, // No parent ID
                    command.OwnerId,
                    DateTime.UtcNow));
            }
            else
            {
                // Handle creation of a sub-organization
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
                    command.ParentId.Value
                );

                parent.AddSubOrganization(organization);
                await _organizationRepository.UpdateAsync(root);

                // Initialize an empty gallery for the sub-organization
                await _organizationGalleryRepository.AddImageAsync(organization.Id, new GalleryImage(Guid.NewGuid(), "Default Image URL", DateTime.UtcNow));

                // Add the creator as a member with the "Creator" role
                var creatorRole = organization.Roles.SingleOrDefault(r => r.Name == "Creator");
                if (creatorRole == null)
                {
                    throw new RoleNotFoundException("Creator");
                }

                var creatorMember = new User(identity.Id, creatorRole);
                await _organizationMembersRepository.AddMemberAsync(organization.Id, creatorMember);

                await _messageBroker.PublishAsync(new OrganizationCreated(
                    organization.Id,
                    organization.Name,
                    organization.Description,
                    command.RootId.Value,
                    command.ParentId.Value,
                    command.OwnerId,
                    DateTime.UtcNow));
            }
        }
    }
}
