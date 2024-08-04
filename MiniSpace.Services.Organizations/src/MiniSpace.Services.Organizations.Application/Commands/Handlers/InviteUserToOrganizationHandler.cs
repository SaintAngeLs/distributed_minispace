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
    public class InviteUserToOrganizationHandler : ICommandHandler<InviteUserToOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public InviteUserToOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(InviteUserToOrganization command, CancellationToken cancellationToken)
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

            var user = await _organizationMembersRepository.GetMemberAsync(organization.Id, identity.Id);
            if (user == null || !user.HasPermission(Permission.InviteUsers))
            {
                throw new UnauthorizedAccessException("User does not have permission to invite users.");
            }

            organization.InviteUser(command.UserId);
            await _organizationRepository.UpdateAsync(organization);
            await _messageBroker.PublishAsync(new UserInvitedToOrganization(
                command.OrganizationId, 
                command.UserId, 
                DateTime.UtcNow));
        }
    }
}
