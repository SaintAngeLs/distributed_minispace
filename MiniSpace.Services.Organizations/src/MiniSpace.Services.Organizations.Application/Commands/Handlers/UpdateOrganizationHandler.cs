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
    public class UpdateOrganizationHandler : ICommandHandler<UpdateOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public UpdateOrganizationHandler(IOrganizationRepository organizationRepository, IAppContext appContext, IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
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
            if (existingOrganization != null)
            {
                // Update existing organization
                existingOrganization.UpdateDetails(command.Name, command.Description, command.Settings, command.BannerUrl, command.ImageUrl);
                await _organizationRepository.UpdateAsync(existingOrganization);
            }
            else
            {
                // Create new organization
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
                    command.ImageUrl);

                parent.AddSubOrganization(newOrganization);
                await _organizationRepository.UpdateAsync(root);
            }

            await _messageBroker.PublishAsync(new OrganizationUpserted(
                command.OrganizationId, 
                existingOrganization != null, 
                DateTime.UtcNow));
        }
    }
}
