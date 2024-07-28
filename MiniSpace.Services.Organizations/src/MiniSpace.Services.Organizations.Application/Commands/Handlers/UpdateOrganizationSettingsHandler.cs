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
    public class UpdateOrganizationSettingsHandler : ICommandHandler<UpdateOrganizationSettings>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public UpdateOrganizationSettingsHandler(
            IOrganizationRepository organizationRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateOrganizationSettings command, CancellationToken cancellationToken)
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

            var user = await _organizationRepository.GetMemberAsync(organization.Id, identity.Id);
            if (user == null || !user.HasPermission(Permission.EditOrganizationDetails))
            {
                throw new UnauthorizedAccessException("User does not have permission to update organization settings.");
            }

            organization.UpdateSettings(command.Settings);
            await _organizationRepository.UpdateAsync(organization);
            await _messageBroker.PublishAsync(new OrganizationSettingsUpdated(
                organization.Id, 
                command.Settings, 
                DateTime.UtcNow));
        }
    }
}
