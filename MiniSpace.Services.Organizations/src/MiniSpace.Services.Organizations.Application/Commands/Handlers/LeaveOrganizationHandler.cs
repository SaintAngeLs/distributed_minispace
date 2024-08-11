using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Exceptions;
using MiniSpace.Services.Organizations.Application.Events;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;

namespace MiniSpace.Services.Organizations.Application.Commands.Handlers
{
    public class LeaveOrganizationHandler : ICommandHandler<LeaveOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;
        private readonly IEventDispatcher _eventDispatcher;

        public LeaveOrganizationHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository,
            IEventDispatcher eventDispatcher)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
            _eventDispatcher = eventDispatcher;
        }

        public async Task HandleAsync(LeaveOrganization command, CancellationToken cancellationToken)
        {
            // Retrieve the organization
            var organization = await _organizationRepository.GetAsync(command.OrganizationId);
            if (organization == null)
            {
                throw new OrganizationNotFoundException(command.OrganizationId);
            }

            // Retrieve the member
            var existingMember = await _organizationMembersRepository.GetMemberAsync(command.OrganizationId, command.UserId);
            if (existingMember == null)
            {
                throw new UserNotMemberException(command.UserId, command.OrganizationId);
            }

            // Remove the user from the organization
            await _organizationMembersRepository.DeleteMemberAsync(command.OrganizationId, command.UserId);

            // Publish event
            await _eventDispatcher.PublishAsync(new UserRemovedFromOrganization(command.OrganizationId, command.UserId, DateTime.UtcNow));
        }
    }
}
