using Convey.CQRS.Commands;
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
    public class CreateOrganizationHandler : ICommandHandler<CreateOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public CreateOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
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
